﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlockChain;
using BlockChain.Data;
using Consensus;
using Infrastructure;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using System.Linq;
using Miner.Data;
using Wallet.core.Data;

namespace Miner
{
	//TODO: refactor duplication
	using ContractFunction = FSharpFunc<Tuple<byte[], byte[], FSharpFunc<Types.Outpoint, FSharpOption<Types.Output>>>, FSharpResult<Tuple<FSharpList<Types.Outpoint>, FSharpList<Types.Output>, byte[]>, string>>;
	using UtxoLookup = FSharpFunc<Types.Outpoint, FSharpOption<Types.Output>>;

	public class MinerManager : ResourceOwner
    {
        readonly TransactionQueue _TransactionQueue = new TransactionQueue();
        readonly List<TransactionValidation.PointedTransaction> _ValidatedTxs = new List<TransactionValidation.PointedTransaction>();

        readonly HashSet _ActiveContracts = new HashSet();
		List<Tuple<Types.Outpoint, Types.Output>> _UtxoSet;

		BlockChain.BlockChain _BlockChain;
        EventLoopMessageListener<BlockChainMessage> _BlockChainListener;

        readonly Hasher _Hasher = new Hasher();

		public uint Difficulty
		{
			get
			{
				return _Hasher.Difficulty;
			}
			set
			{
				_Hasher.Difficulty = value;
			}
		}

        public event Action<Types.Block> OnMined;

        public int TxsPerBlockLimit { get; set; }

        Types.BlockHeader _Header;
        Types.Transaction _Coinbase;
        Address _Address;

        public MinerManager(BlockChain.BlockChain blockChain, Address address)
        {
            TxsPerBlockLimit = 100; //TODO
            Difficulty = 12; //TODO: when set to 10, becomes apparent a issue of 'tx race condition' (i.e. when using a secure contract to send token, two subsequent txs are dispatched) in which the miner is reset after mining a block with 1st tx and 2nd is never gets mined

			_Address = address;
			_BlockChain = blockChain;
            _Hasher.OnMined += DispatchBlock;
			OwnResource(_Hasher);

			_BlockChainListener = new EventLoopMessageListener<BlockChainMessage>(OnBlockChainMessage, "Miner Consumer");
			OwnResource(MessageProducer<BlockChainMessage>.Instance.AddMessageListener(_BlockChainListener));

            Reset();
		}

        async void Reset()
        {
            _TransactionQueue.Clear();
            _ValidatedTxs.Clear();

            var acsList = await new GetActiveContractsAction().Publish();
            _UtxoSet = await new GetUTXOSetAction2().Publish();

            _ActiveContracts.Clear();
            acsList.ForEach(t => _ActiveContracts.Add(t.Hash));

			Populate();
		}

        void Populate()
        {
			_BlockChainListener.Pause();

            _TransactionQueue.Clear();

            lock (_BlockChain.memPool.TxPool)
            {
				foreach (var item in _BlockChain.memPool.TxPool)
                {
                    _TransactionQueue.Push(item.Value);
                }
            }

            MinerTrace.Information($"Populated, having {_TransactionQueue.Count} txs");

			RecalculateHeader();

            _BlockChainListener.Continue();
		}

		void OnBlockChainMessage(BlockChainMessage m)
		{
			_Hasher.Pause("BlockChain message");

			if (m is TxMessage)
			{
                MinerTrace.Information($"Got tx ({((TxMessage)m).State})");

                if (((TxMessage)m).State == TxStateEnum.Unconfirmed)
                    _TransactionQueue.Push(((TxMessage)m).Ptx);

                RecalculateHeader();
			}
			else if (m is BlockMessage)
			{
				MinerTrace.Information("Got bk");

                Reset();
			}
		}

        bool IsTransactionValid(TransactionValidation.PointedTransaction ptx)
        {
			if (!HasUtxos(ptx))
			{
				MinerTrace.Information("could not validate tx - utxo missing");
				return false;
			}

            var utxoLookup = UtxoLookup.FromConverter(outpoint =>
			{
                var outputs = _UtxoSet.Where(t => t.Item1.Equals(outpoint)).Select(t => t.Item2);
                return !outputs.Any() ? FSharpOption<Types.Output>.None : new FSharpOption<Types.Output>(outputs.First());
            });

            var contractLookup = FSharpFunc<byte[], FSharpOption<ContractFunction>>.FromConverter(contractHash =>
            {
                if (!_ActiveContracts.Contains(contractHash))
                    return FSharpOption<ContractFunction>.None;

				try
				{
                    var code = new GetContractCodeAction(contractHash).Publish().Result;

					//TODO: module name
					var extration = ContractExamples.FStarExecution.extract(System.Text.Encoding.ASCII.GetString(code));

					if (FSharpOption<string>.get_IsNone(extration))
					{
						MinerTrace.Information("Could not extract contract");
						return null;
					}

					var compilation = ContractExamples.FStarExecution.compile(extration.Value);

					if (FSharpOption<byte[]>.get_IsNone(compilation))
					{
						MinerTrace.Information("Could not complie contract");
						return null;
					}

					return ContractExamples.FStarExecution.deserialize(compilation.Value).Value.Item1;
				}
				catch (Exception e)
				{
					MinerTrace.Error("Could not compile contract " + Convert.ToBase64String(contractHash), e);
                    return null;
				}
            });

            if (!TransactionValidation.validateNonCoinbaseTx(
                ptx,
                utxoLookup,
                contractLookup
            ))
            {
                MinerTrace.Information("could not validate tx");
                return false;
            }

			MinerTrace.Information("validated tx");

			//TODO: memory management issues. trying to explicitly collect DynamicMethods
			GC.Collect();
			GC.WaitForPendingFinalizers();

			return true;
        }

        //TODO: refactore for efficiency
        bool HasUtxos(TransactionValidation.PointedTransaction ptx)
        {
            foreach (var input in ptx.pInputs)
            {
				foreach (var tx in _ValidatedTxs)
				{
                    foreach (var txInput in tx.pInputs)
                    {
                        if (input.Item1.Equals(txInput))
                        {
                            return false;
                        }
                    }
				}
			}

			foreach (var input in ptx.pInputs)
			{
				foreach (var tx in _ValidatedTxs)
				{
					var txHash = Merkle.transactionHasher.Invoke(TransactionValidation.unpoint(ptx));
                    for (var i = 0; i < tx.outputs.Length; i++)
					{
						var outpoint = new Types.Outpoint(txHash, (uint)i);

                        if (input.Item1.Equals(outpoint))
                        {
                            return true;
                        }
                    }
                }
            }

			foreach (var input in ptx.pInputs)
            {
                if (!_UtxoSet.Any(t => t.Item1.Equals(input.Item1)))
                {
                    return false;
                }
            }

            return true;
		}

		void HandleTx(TransactionValidation.PointedTransaction ptx)
		{
			//TODO: try simplify using hash from message
			var txHash = Merkle.transactionHasher.Invoke(TransactionValidation.unpoint(ptx));

            var activationSacrifice = 0UL;

            for (var i = 0; i < ptx.outputs.Length; i++)
            {
				var output = ptx.outputs[i];

				if (output.@lock.IsContractSacrificeLock)
				{
					if (!output.spend.asset.SequenceEqual(Tests.zhash))
						continue; // not Zen

					var contractSacrificeLock = (Types.OutputLock.ContractSacrificeLock)output.@lock;

					if (contractSacrificeLock.IsHighVLock)
						continue; // not current version

                    if (contractSacrificeLock.Item.lockData.Length == 0)
					{
						activationSacrifice += output.spend.amount;
					}
				}

				//todo: fix  to exclude CSLocks&FLocks, instead of including by locktype
				if (output.@lock.IsPKLock || output.@lock.IsContractLock)
				{
					var outpoint = new Types.Outpoint(txHash, (uint)i);
					_UtxoSet.Add(new Tuple<Types.Outpoint, Types.Output>(outpoint, output));
				}
			}

			if (FSharpOption<Types.ExtendedContract>.get_IsSome(ptx.contract) && !ptx.contract.Value.IsHighVContract)
			{
				var codeBytes = ((Types.ExtendedContract.Contract)ptx.contract.Value).Item.code;
				var contractHash = Merkle.innerHash(codeBytes);
				var contractCode = System.Text.Encoding.ASCII.GetString(codeBytes);

                if (!_ActiveContracts.Contains(contractHash))
                {
                    if (activationSacrifice > ActiveContractSet.KalapasPerBlock(contractCode))
                    {
						try
						{
                            var compiledCodeOpt = ContractExamples.FStarExecution.compile(contractCode);

                            if (FSharpOption<byte[]>.get_IsSome(compiledCodeOpt))
                            {
                                _ActiveContracts.Add(contractHash);
                            }
						}
						catch (Exception e)
						{
							MinerTrace.Error("Could not compile contract " + Convert.ToBase64String(contractHash), e);
						}
					}
                }
			}
		}

		void RecalculateHeader()
        {
			if (_BlockChain.Tip == null)
			{
				return;
			}

            if (_TransactionQueue.IsStuck)
                MinerTrace.Information("Queue is stuck. count = " + _TransactionQueue.Count);

			while (!_TransactionQueue.IsStuck && _ValidatedTxs.Count < TxsPerBlockLimit)
			{
				var ptx = _TransactionQueue.Take();

				if (IsTransactionValid(ptx))
				{
					_ValidatedTxs.Add(ptx);

					_TransactionQueue.Remove();

                    HandleTx(ptx);
                }
				else
				{
                    MinerTrace.Information("Tx invalid");
					_TransactionQueue.Next();
				}
			}

            if (_ValidatedTxs.Count == 0)
            {
				MinerTrace.Information("No txs");
				return; // don't allow empty blocks
            }

            CalculateCoinbase();

            var txs = ListModule.OfSeq(FSharpList<Types.Transaction>.Cons(_Coinbase, ListModule.OfSeq(_ValidatedTxs.Select(TransactionValidation.unpoint))));

            _Header = new Types.BlockHeader(
                0,
				_BlockChain.Tip.Key,
				_BlockChain.Tip.Value.header.blockNumber + 1,
                Merkle.merkleRoot(
                    new byte[] { },
                    Merkle.transactionHasher,
                    txs
                ),
				new byte[] { },
				new byte[] { },
                ListModule.Empty<byte[]>(),
                DateTime.Now.ToUniversalTime().Ticks,
                Difficulty,
                new byte[12]
            );

            MinerTrace.Information($"Mining block number {_BlockChain.Tip.Value.header.blockNumber + 1} with {_ValidatedTxs.Count()} txs");

			_Hasher.SetHeader(_Header);
            _Hasher.Continue();
        }

        void CalculateCoinbase()
        {
            var reward = 1000u;

            var outputs = new List<Types.Output>
            {
                new Types.Output(Types.OutputLock.NewPKLock(_Address.Bytes), new Types.Spend(Tests.zhash, reward))
            };

            var witness = new List<byte[]>
            {
                BitConverter.GetBytes(_BlockChain.Tip.Value.header.blockNumber + 1)
            };

            _Coinbase = new Types.Transaction(
                0,
                ListModule.Empty<Types.Outpoint>(),
                ListModule.OfSeq(witness),
                ListModule.OfSeq(outputs),
                FSharpOption<Types.ExtendedContract>.None
            );
        }

        void DispatchBlock()
        {
            try
            {
                _Hasher.Pause("dispatching block");

                MinerTrace.Information($"dispatching block with {_ValidatedTxs.Count()} txs");

                var txs = FSharpList<Types.Transaction>.Cons(_Coinbase, ListModule.OfSeq(_ValidatedTxs.Select(TransactionValidation.unpoint)));
                var block = new Types.Block(_Header, txs);


                var result = new HandleBlockAction(block).Publish().Result.BkResultEnum;

                if (result == BlockVerificationHelper.BkResultEnum.Accepted)
                {
                    if (OnMined != null)
                        OnMined(block);
                }
                else
                {
                    Reset();
                }

                MinerTrace.Information($"  block {block.header.blockNumber} is " + result);
            } catch (Exception e)
            {
                MinerTrace.Error("Dispatch block exception", e);
            }
		}
	}
}
