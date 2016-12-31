using System;
using System.Collections.Generic;
using Store;
using BlockChain.Store;
using Consensus;
using Infrastructure;

namespace BlockChain
{
	public class BlockChainAddTransactionOperation
	{
		private readonly TransactionContext _TransactionContext;
		private readonly Keyed<Types.Transaction> _NewTransaction; //TODO: or just use var key
		private readonly TxMempool _TxMempool;
		private readonly TxStore _TxStore;
		private readonly UTXOStore _UTXOStore;
		private Dictionary<Types.Outpoint, InputLocationEnum> _InputLocations;

		private enum InputLocationEnum
		{
			Mempool,
			TxStore,
		}

		public enum Result
		{
			Added,
			AddedOrphan,
			Rejected
		}

		public BlockChainAddTransactionOperation(
			TransactionContext transactionContext, 
			Keyed<Types.Transaction> transaction, 
			TxMempool txMempool, 
			TxStore txStore, 
			UTXOStore utxoStore
		)
		{
			_InputLocations = new Dictionary<Types.Outpoint, InputLocationEnum>();
			_TransactionContext = transactionContext;
			_NewTransaction = transaction;
			_TxMempool = txMempool;
			_TxStore = txStore;
			_UTXOStore = utxoStore;
		}

		public Result Start()
		{
			if (!IsValid() || IsInMempool() || IsInTxStore())
			{
				BlockChainTrace.Information("not valid/in mempool/in txstore");
				return Result.Rejected;
			}

			if (IsMempoolContainsSpendingInput())
			{
				BlockChainTrace.Information("Mempool contains spending input");
				return Result.Rejected;
			}

			if (IsOrphan())
			{
				BlockChainTrace.Information("Added as orphan");
				_TxMempool.Add(_NewTransaction, true);
				return Result.AddedOrphan;
			}

			//TODO: 5. For each input, if the referenced transaction is coinbase, reject if it has fewer than COINBASE_MATURITY confirmations.

			if (!IsValidInputs())
			{
				BlockChainTrace.Information("invalid inputs");
				return Result.Rejected;
			}

			//TODO: 7. Apply fee rules. If fails, reject
			//TODO: 8. Validate each input. If fails, reject

			BlockChainTrace.Information("Transaction added to mempool");
			_TxMempool.Add(_NewTransaction);

			foreach (var transaction in _TxMempool.GetOrphansOf(_NewTransaction)) 
			{
				BlockChainTrace.Information("Start with orphan");
				new BlockChainAddTransactionOperation(
					_TransactionContext, 
					transaction, 
					_TxMempool,
					_TxStore,
					_UTXOStore
				).Start();
			}

			return Result.Added;
		}

		private bool IsValid()
		{
			return true;
		}

		private bool IsInMempool()
		{
			var result = _TxMempool.ContainsKey(_NewTransaction.Key);

			BlockChainTrace.Information($"IsInMempool = {result}");

			return result;
		}

		private bool IsInTxStore()
		{
			var result = _TxStore.ContainsKey(_TransactionContext, _NewTransaction.Key);

			BlockChainTrace.Information($"IsInTxStore = {result}");

			return result;
		}

		private bool IsMempoolContainsSpendingInput()
		{
			foreach (var inputs in _NewTransaction.Value.inputs)
			{
				if (_TxMempool.ContainsInputs(_NewTransaction))
				{
					return true;
				}
			}

			return false;
		}

		private bool IsOrphan()
		{
			foreach (var input in _NewTransaction.Value.inputs)
			{
				if (_TxMempool.ContainsKey(input.txHash))
				{
					_InputLocations[input] = InputLocationEnum.Mempool;
				}
				else if (_TxStore.ContainsKey(_TransactionContext, input.txHash))
				{
					_InputLocations[input] = InputLocationEnum.TxStore;
				}
				else {
					return true;
				}
			}

			return false;
		}

		private bool IsValidInputs()
		{
			foreach (var input in _NewTransaction.Value.inputs)
			{
				if (!ParentOutputExists(input))
				{
					BlockChainTrace.Information("parent output does not exist");
					return false;
				}

				if (ParentOutputSpent(input))
				{
					BlockChainTrace.Information("parent output spent");
					return false;
				}
			}

			return true;
		}

		private bool ParentOutputExists(Types.Outpoint input)
		{
			Keyed<Types.Transaction> parentTransaction = null;

			switch (_InputLocations[input])
			{
				case InputLocationEnum.Mempool:
					parentTransaction = _TxMempool.Get(input.txHash);
					break;
				case InputLocationEnum.TxStore:
					parentTransaction = _TxStore.Get(_TransactionContext, input.txHash);
					break;
			}

			if (input.index >= parentTransaction.Value.outputs.Length)
			{
				BlockChainTrace.Information("Input index not found");
				return false;
			}

			return true;
		}

		private bool ParentOutputSpent(Types.Outpoint input)
		{
			byte[] newArray = new byte[input.txHash.Length + 1];
			input.txHash.CopyTo(newArray, 1);
			newArray[input.txHash.Length] = (byte)input.index;

			if (_InputLocations[input] == InputLocationEnum.TxStore && !_UTXOStore.ContainsKey(_TransactionContext, newArray))
			{
				BlockChainTrace.Information("Output has been spent");
				return true;
			}

			return false;
		}
	}
}
