using System;
using Wallet.core;
using Wallet.Domain;
using System.Linq;
using Infrastructure;

namespace Wallet
{
	public class BalancesController : Singleton<BalancesController>
	{
		AssetDeltas _AssetDeltasSent = new AssetDeltas();
		AssetDeltas _AssetDeltasRecieved = new AssetDeltas();
		AssetDeltas _AssetDeltasTotal = new AssetDeltas();
		TxDeltaItemsEventArgs _TxDeltas;
		ILogView _LogView;
		IAssetsView _AssetsView;

		public BalancesController()
		{
			Asset = Consensus.Tests.zhash;
		}

		public ILogView LogView
		{
			set
			{
				_LogView = value;
				_TxDeltas = App.Instance.Wallet.TxDeltaList;
				Apply();
				App.Instance.Wallet.OnReset += delegate { value.Clear(); };
				App.Instance.Wallet.OnItems += a => {
                    _TxDeltas = App.Instance.Wallet.TxDeltaList;
                    Apply();
                };
			}
		}

		public IAssetsView AssetsView
		{
			set
			{
				_AssetsView = value;

                App.Instance.Wallet.AssetsMetadata.AssetMatadataChanged += t => _AssetsView.AssetUpdated = t; //TODO: streamline this

                _AssetsView.Assets = App.Instance.Wallet.AssetsMetadata.GetAssetMatadataList();
			}
		}

		byte[] _Asset;
		public byte[] Asset
		{
			set
			{
				_Asset = value;

				if (_LogView != null)
				{
					_LogView.Clear();
					Apply();
				}
			}
		}

		void Apply()
		{
			_AssetDeltasSent.Clear();
			_AssetDeltasRecieved.Clear();
			_AssetDeltasTotal.Clear();

			Gtk.Application.Invoke(delegate
			{
				_TxDeltas.ForEach(u => u.AssetDeltas.Where(b => b.Key.SequenceEqual(_Asset)).ToList().ForEach(b =>
				{
					AddToTotals(b.Key, b.Value);

					var total = _AssetDeltasTotal.ContainsKey(_Asset) ? _AssetDeltasTotal[_Asset] : 0;
                    var decTotal = _Asset.SequenceEqual(Consensus.Tests.zhash) ? new Zen(total).Value : total;
                    var decValue = _Asset.SequenceEqual(Consensus.Tests.zhash) ? new Zen(b.Value).Value : b.Value;

                    _LogView.AddLogEntryItem(u.TxHash, new LogEntryItem(
					Math.Abs(decValue),
					b.Value < 0 ? DirectionEnum.Sent : DirectionEnum.Recieved,
					b.Key,
					u.Time,
					"TODO",
                    u.TxState.ToString(), //Convert.ToBase64String(u.TxHash),
					decTotal
					));
				}));

				UpdateTotals();
			});
		}

		void AddToTotals(byte[] asset, long amount)
		{
			AddToTotals(amount < 0 ? _AssetDeltasSent : _AssetDeltasRecieved, asset, amount);
			AddToTotals(_AssetDeltasTotal, asset, amount);
		}

		void AddToTotals(AssetDeltas assetDeltas, byte[] asset, long amount)
		{
			if (!assetDeltas.ContainsKey(asset))
				assetDeltas[asset] = 0;

			assetDeltas[asset] += amount;
		}

		void UpdateTotals()
		{
			if (_LogView == null)
				return;

			var sent = _AssetDeltasSent.ContainsKey(_Asset) ? new Zen(_AssetDeltasSent[_Asset]).Value : 0;
			var recieved = _AssetDeltasRecieved.ContainsKey(_Asset) ? new Zen(_AssetDeltasRecieved[_Asset]).Value : 0;
			var total = _AssetDeltasTotal.ContainsKey(_Asset) ? new Zen(_AssetDeltasTotal[_Asset]).Value : 0;

			_LogView.Totals(Convert.ToDecimal(sent), Convert.ToDecimal(recieved), Convert.ToDecimal(total));
		}
	}
}

