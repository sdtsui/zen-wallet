﻿using System;
using CLRCLI.Widgets;
using CLRCLI;
using System.IO;
using System.Collections.Generic;
using Wallet.core.Data;
using Infrastructure;
using Zen.Data;
using System.Globalization;
using Wallet.Constants;
using Wallet.core;
using Network;
using System.Linq;
using System.Threading.Tasks;
using BlockChain.Data;

namespace Zen
{
	class TUI
	{
		static ListBox listTrace;
		static Dialog dialog;
		static ListBox listMenu;
		static string currentMenu { get; set; }

		public static void Start (App app, String script)
		{
			var root = new RootWindow();
			var options = new Dictionary<string, List<string>>();

			options["main"] = new List<string>();
			options["main"].Add("Connect");
			options["main"].Add("Start GUI");
			options["main"].Add("Wallet Menu");
			options["main"].Add("BlockChain Menu");
			options["main"].Add("Miner Menu");
			options["main"].Add("Tests");
			options["main"].Add("Reset Blockchain DB");
			options["main"].Add("Reset Wallet DB");
			options["main"].Add("Generate Graph");
			options["main"].Add("Active Contract Set");
			options["main"].Add("Stop");
			options["main"].Add("Exit");

			options["wallet"] = new List<string>();
		//	options["wallet"].Add("Reset");
		//	options["wallet"].Add("Sync");
			options["wallet"].Add("Add Genesis UTXO");
			options["wallet"].Add("List Keys");
			options["wallet"].Add("Import Test Key");
			options["wallet"].Add("Get Receive Address");
			options["wallet"].Add("My Wallet");
			options["wallet"].Add("Send Dialog");
			options["wallet"].Add("Back");

			options["blockchain"] = new List<string>();
			options["blockchain"].Add("Add Genesis");
			options["blockchain"].Add("Back");

			options["miner"] = new List<string>();
			options["miner"].Add("Start Miner");
			options["miner"].Add("Stop Miner");
			options["miner"].Add("Mine Block");
			options["miner"].Add("Back");

			options["tests"] = new List<string>();

			foreach (var scriptFile in Directory.GetFiles("Scripts"))
			{
				var fileInfo = new FileInfo(scriptFile);

				if (fileInfo.Extension == ".fs")
					options["tests"].Add(fileInfo.Name);
			}
			options["tests"].Add("Back");


			var actions = new Dictionary<string, Action<string>>();
			Action<string> menu = (s) =>
			{
				currentMenu = s;
				listMenu.Items.Clear();
				listMenu.SelectedIndex = 0;
				options[currentMenu].ForEach(t => listMenu.Items.Add(t));
			};

			dialog = new Dialog(root) { Text = string.IsNullOrEmpty(app.Settings.NetworkProfile) ? "Zen" : app.Settings.NetworkProfile.Replace(".json", ""), Width = 75, Height = 19, Top = 2, Left = 2, Border = BorderStyle.Thick };

			#region Send Dialog
			var sendDialog = new Dialog(root) { Text = "Send", Width = 70, Height = 18, Top = 4, Left = 4, Border = BorderStyle.Thick, Visible = false };

            new Label(sendDialog) { Top = 1, Left = 1, Width = 66, Text = "Destination" };
			var address = new SingleLineTextbox(sendDialog) { Top = 3, Left = 1, Width = 65 };
			
            new Label(sendDialog) { Top = 5, Left = 1, Width = 66, Text = "Amount" };
			var amount = new SingleLineTextbox(sendDialog) { Top = 7, Left = 1, Width = 65 };

			new Label(sendDialog) { Top = 9, Left = 1, Width = 66, Text = "Data" };
			var data = new SingleLineTextbox(sendDialog) { Top = 11, Left = 1, Width = 65 };

			var sendDialogSendButton = new Button(sendDialog) { Top = 14, Left = 1, Width = 15, Text = "Send" };
			var sendDialogCloseButton = new Button(sendDialog) { Top = 14, Left = 20, Width = 15, Text = "Close" };
			var status = new Label(sendDialog) { Top = 16, Left = 1, Width = 40, Text = "", Background = ConsoleColor.Black };

			sendDialogCloseButton.Clicked += (sender, e) =>
			{
				sendDialog.Hide();
				dialog.Show();
			};

			sendDialogSendButton.Clicked += (sender, e) =>
			{
				ulong _amount;

				try
				{
					_amount = ulong.Parse(amount.Text);
				}
				catch
				{
					status.Text = "Invalid amount";
					return;
				}

				Address _address;

				try
				{
					_address = new Address(address.Text);
				}
				catch
				{
					status.Text = "Invalid address";
					return;
				}

                if (string.IsNullOrEmpty(data.Text))
                {
                    if (!app.Spend(_address, _amount))
                    {
                        status.Text = "Could not spend";
                        return;
                    }
                }
                else
                {
     //               if (!app.SendContract(_address.Bytes, _amount, Convert.FromBase64String(data.Text)))
					//{
					//	status.Text = "Could not spend";
					//	return;
					//}
                }

				status.Text = "Success";
			};

			Action showSendDialog = () => {
				status.Text = "";
				dialog.Hide();
				sendDialog.Show();
				address.SetFocus();
			};
			#endregion

			#region Miner Dialog
			var minerDialog = new Dialog(root) { Text = "Miner", Width = 70, Height = 18, Top = 2, Left = 6, Border = BorderStyle.Thick, Visible = false };
			var radioMinerEnabled = new RadioButton(minerDialog) { Top = 1, Left = 1, Id = "minerIsEnabled", Text = "Enabled" };
			var radioMinerDisabled = new RadioButton(minerDialog) { Top = 2, Left = 1, Id = "minerIsEnabled", Text = "Disabled" };
			new Label(minerDialog) { Top = 4, Left = 1, Width = 10, Text = "Difficulty" };
			var difficulty = new SingleLineTextbox(minerDialog) { Top = 4, Left = 15, Width = 10 };
			var minerDialogMinerButton = new Button(minerDialog) { Top = 1, Left = 32, Width = 15, Text = "Mine Now" };
			var minerDialogCloseButton = new Button(minerDialog) { Top = 1, Left = 50, Width = 18, Text = "Apply and Close" };
			var minerLog = new ListBox(minerDialog) { Top = 6, Left = 2, Width = 66, Height = 11, Border = BorderStyle.Thin };

			Func<MinerLogData, string> minerLogData = log =>
			{
				return $"Block #{log.BlockNumber} {log.Status} with {log.Transactions} txs, in {log.TimeToMine} seconds";
			};

			app.NodeManager.Miner.OnMinedBlock += log => minerLog.Items.Add(minerLogData(log));

			minerDialogCloseButton.Clicked += (sender, e) =>
			{
				var miner = app.NodeManager.Miner;

				miner.Difficulty = int.Parse(difficulty.Text);
				miner.Enabled = radioMinerEnabled.Checked;

				minerDialog.Hide();
				dialog.Show();
			};

			minerDialogMinerButton.Clicked += (sender, e) =>
			{
				app.MineTestBlock();
			};

			Action showMinerDialog = () =>
			{
				var miner = app.NodeManager.Miner;
				difficulty.Text = miner.Difficulty.ToString();

				if (miner.Enabled)
					radioMinerEnabled.Checked = true;
				else
					radioMinerDisabled.Checked = true;

				minerLog.Items.Clear();
				foreach (var log in app.MinerLogData)
				{
					minerLog.Items.Add(minerLogData(log));
				}

				dialog.Hide();
				minerDialog.Show();
			};
			#endregion

			actions["main"] = async a =>
			{
				switch (a)
				{
					case "Wallet Menu":
						menu("wallet");
						break;
					case "BlockChain Menu":
						menu("blockchain");
						break;
					case "Miner Menu":
						showMinerDialog();
						break;
					case "Connect":
						await app.Connect();
						break;
					case "Start GUI":
						app.GUI();
						root.Detach();
						root.Run();
						break;
					case "Reset Blockchain":
						app.ResetBlockChainDB();
						break;
					case "Reset Wallet":
						app.ResetWalletDB();
						break;
					case "Generate Graph":
						app.Dump();
						break;
					case "Active Contract Set":
						options["acs"] = new List<string>();

						foreach (var contractData in new GetActiveContactsAction().Publish().Result)
						{
							var info = new Address(contractData.Hash, AddressType.Contract) + " " + contractData.LastBlock;

						//	info += app.GetTotalAssets(contractData.Hash);

							options["acs"].Add(info);
						}
						options["acs"].Add("Back");

                        menu("acs");
						break;
					case "Reset Blockchain DB":
						app.ResetBlockChainDB();
						break;
					case "Reset Wallet DB":
						app.ResetWalletDB();
						break;
					case "Stop":
						app.Stop();
						break;
					case "Tests":
						menu("tests");
						break;
					case "Exit":
						app.Dispose();
						root.Detach();
						break;
				}
			};

			actions["blockchain"] = a =>
			{
				switch (a)
				{
					case "Add Genesis":
						app.AddGenesisBlock();
						break;
					case "Back":
						menu("main");
						break;
				}
			};

			actions["miner"] = a =>
			{
				switch (a)
				{
					case "Start Miner":
						app.MinerEnabled = true;
						break;
					case "Stop Miner":
						app.MinerEnabled = false;
						break;
					case "Mine Block":
						app.MineTestBlock();
						break;
					case "Back":
						menu("main");
						break;
				}
			};

			actions["tests"] = a =>
			{
				switch (a)
				{
					case "Back":
						menu("main");
						break;
					default:
						object result;
						ScriptRunner.Execute(app, Path.Combine("Scripts", a), out result);
						listTrace.Items.Add(result);	
						break;
				}
			};

			actions["acs"] = a =>
			{
				switch (a)
				{
					case "Back":
                        menu("main");
						break;
					default:
						Console.WriteLine(new GetContractCodeAction(new Address(a).Bytes).Publish().Result);
						break;
				}
			};

			var wallet_mode = "";
			actions["wallet"] = a =>
			{
				switch (a)
				{
					case "Add Genesis UTXO":
						wallet_mode = a;
						listMenu.Items.Clear();

						foreach (var output in JsonLoader<Outputs>.Instance.Value.Values)
						{
							listMenu.Items.Add(output.Amount + " " + output.Key);
						}
						listMenu.Items.Add("Back");
						break;
					case "List Keys":
						listMenu.Items.Clear();

						foreach (var key in app.WalletManager.GetKeys())
						{
							string info = key.Used ? "used" : "ununsed";

							if (key.Change)
								info += ",change";
									
							listMenu.Items.Add(info + " " + key.PrivateAsString);
						}
						listMenu.Items.Add("Back");
						break;
					case "Import Test Key":
						wallet_mode = a;

						listMenu.Items.Clear();

						foreach (var key in  JsonLoader<Keys>.Instance.Value.Values)
						{
							listMenu.Items.Add(key);
						}
						listMenu.Items.Add("Back");
						break;
					//case "Reset":
					//	app.ResetWallet();
					//	break;
					//case "Import":
					//	app.ImportWallet();
					//	break;
					case "Get Receive Address":
						listTrace.Items.Add(app.WalletManager.GetUnusedKey().Address.ToString());
						break;
					case "My Wallet":
						listMenu.Items.Clear();

						foreach (var txDelta in app.WalletManager.TxDeltaList)
						{
							listMenu.Items.Add(GetTxDeltaInfo(app, txDelta));
						}
						listMenu.Items.Add("Back");
						break;
					case "Send Dialog":
						showSendDialog();
						break;
					case "Back":
						menu("main");
						break;
					default:
						switch (wallet_mode)
						{
							case "Add Genesis UTXO":
								foreach (var output in JsonLoader<Outputs>.Instance.Value.Values)
								{
									if (a == output.Amount + " " + output.Key)
									{
										app.WalletManager.Import(Key.Create(output.Key));
									}
								}
								break;
							case "Import Test Key":
								app.WalletManager.Import(Key.Create(a));
								break;
						}
						break;
				}
			};

		//	dialogMenu.Show();
			listMenu = new ListBox(dialog) { Top = 1, Left = 1, Width = 72, Height = 6, Border = BorderStyle.Thin };
			listTrace = new ListBox(dialog) { Top = 8, Left = 1, Width = 72, Height = 11, Border = BorderStyle.Thin };

			menu("main");
			//	app.Settings.InitGenesisBlock = checkboxGenesis.Checked;

			listMenu.Clicked += (object sender, EventArgs e) => {
		//		var option = currentMenu + "_" + ((ListBox)sender).SelectedItem;

				if (actions.ContainsKey(currentMenu))
				{
					actions[currentMenu](((ListBox)sender).SelectedItem as string);
				}
				else
				{
					Console.Write("missing " + currentMenu);
				}
			};

			Action<ResetEventArgs> wallet_OnReset = a =>
			{
				foreach (var txDelta in a.TxDeltaList)
					listTrace.Items.Add(GetTxDeltaInfo(app,txDelta, "Wallet reset"));
			};

			Action<TxDeltaItemsEventArgs> wallet_OnItems = a =>
		   	{
				foreach (var txDelta in a)
					listTrace.Items.Add(GetTxDeltaInfo(app, txDelta, "Wallet item"));
		   	};

			app.WalletOnItemsHandler = wallet_OnItems;
			app.WalletOnResetHandler = wallet_OnReset;

			//	app.Init();
			root.Run();
		}

        static string GetTxDeltaInfo(App app, TxDelta txDelta, string prefix = null)
		{
            string info = (prefix == null ? "" : prefix + ": ") + txDelta.TxState.ToString().Substring(0, 1);
			info += ", " + txDelta.Time.ToString("g", DateTimeFormatInfo.InvariantInfo);

            string assets = string.Empty;

            foreach (var item in txDelta.AssetDeltas)
            {
                assets += (assets == string.Empty ? "" : ", ") + item.Value;
                assets += " " + app.WalletManager.AssetsMetadata.Get(item.Key).Result;
            }

			return info + " " + assets;
		}
						                
		public static void WriteColor(string message, ConsoleColor color)
		{			
			if (listTrace != null)
			{
                try
                {
					var lines = message.Split(new string[] { Environment.NewLine }, StringSplitOptions.None); // just split the thing alreadymessage.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)[0]); // just split the thing already
					var line = lines[0];

					if (line.Length > 75)
					{
						line = message.Substring(0, 75);
						line += "...\n";
					}

					line = line.Replace("{", "");
					line = line.Replace("}", "");

					listTrace.Items.Add(line);

					listTrace.SelectedIndex = listTrace.Items.Count - 1;
                } catch (Exception e) {
					Console.ForegroundColor = color;
					Console.WriteLine("error writing message");
				}
			}
			else
			{
				Console.ForegroundColor = color;

                if (!message.EndsWith("\n"))
                {
                    message += "\n";
                }

				Console.Write(message);
			}
		}
	}
}
