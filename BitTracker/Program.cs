using System;
using System.Collections.Generic;
using Gtk;

namespace BitTracker
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			CoinStats coinStats = BitStats.GetCoinStats("BTC-USD");
			BlockHash blockHash = BitStats.GetBlockStats();

			Application.Init();
			MainWindow win = new MainWindow();
			win.Title = "BitTracker";
			win.KeepAbove = true;

			HBox hbox1 = new HBox(false, 5);
			HBox hbox2 = new HBox(false, 5);
			VBox vbox = new VBox(false, 5);
			win.Add(vbox);
			vbox.PackStart(hbox1, false, false, 10);
			vbox.PackStart(hbox2, false, false, 10);

			float coinStatsChange = ((coinStats.last / coinStats.open) * 100 - 100);
			string coinStatsLabelString = "Opening:\t\t" + coinStats.open.ToString("0.00") + 
			                                                        "\nCurrent:\t\t" + coinStats.last.ToString("0.00") + 
			                                                        "\nChange\t\t" + coinStatsChange.ToString("0.00") + "%" + (coinStatsChange > 0 ? "\u2191" : "\u2193");
			Label coinStatsLabel = new Label();
			coinStatsLabel.Text = coinStatsLabelString;
			Frame coinStatsFrame = new Frame();
			coinStatsFrame.Label = "24-hour Stats";
			coinStatsFrame.Add(coinStatsLabel);
			hbox1.PackStart(coinStatsFrame, false, false, 0);

			List<string> blockStatsLabelString = new List<string>();
			string prevblockHash = blockHash.hash;
			float blockHashTime = blockHash.time / 3600000 - 300;
			blockStatsLabelString.Add(blockHash.hash + "\t" + blockHash.height.ToString() + "\t" + blockHashTime.ToString("00:00"));
			Label blockStatsLabel = new Label();
			blockStatsLabel.Text = "Hash:\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tHeight:\tTime:\n" + blockStatsLabelString[0];
			Frame blockStatsFrame = new Frame();
			blockStatsFrame.Label = "Latest Blocks";
			blockStatsFrame.Add(blockStatsLabel);
			hbox2.PackStart(blockStatsFrame, false, false, 0);

			uint timer = GLib.Timeout.Add(10000, new GLib.TimeoutHandler(() =>
			{
				coinStats = BitStats.GetCoinStats("BTC-USD");
				coinStatsChange = ((coinStats.last / coinStats.open) * 100 - 100);
				coinStatsLabelString = "Opening:\t\t" + coinStats.open.ToString("0.00") + 
				                                                 "\nCurrent:\t\t" + coinStats.last.ToString("0.00") + 
				                                                 "\nChange\t\t" + coinStatsChange.ToString("0.00") + "%" + (coinStatsChange > 0 ? "\u2191" : "\u2193");				coinStatsLabel.Text = coinStatsLabelString;

				blockHash = BitStats.GetBlockStats();
				if (blockHash.hash != prevblockHash)
				{
					blockHashTime = blockHash.time / 3600000 - 300;
					blockStatsLabelString.Insert(0, blockHash.hash + "\t" + blockHash.height.ToString() + "\t" + blockHashTime.ToString("00:00") + "\n");
					prevblockHash = blockHash.hash;
				}
				string toBlockStatsLabel = "Hash:\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tHeight:\tTime:\n";
				foreach (string s in blockStatsLabelString)
				{
					toBlockStatsLabel += s;
				}
				blockStatsLabel.Text = toBlockStatsLabel;

				return true;
			}));

			win.ShowAll();
			Application.Run();
		}
	}
}
