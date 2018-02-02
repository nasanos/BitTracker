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
			BlockList blockHash = BitStats.GetBlockStats();

            // Configure window
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

            // Display 24-hour coin stats
			float coinStatsChange = ((coinStats.last / coinStats.open) * 100 - 100);
			string coinStatsLabelString = "Opening:\t\t" + coinStats.open.ToString("0.00") + 
			                                                        "\nCurrent:\t\t" + coinStats.last.ToString("0.00") + 
			                                                        "\nChange\t\t" + coinStatsChange.ToString("0.00") + "%" + (coinStatsChange > 0 ? "\u2191" : "\u2193");
			Label coinStatsLabel = new Label();
			coinStatsLabel.Text = coinStatsLabelString;
			Frame coinStatsFrame = new Frame();
			coinStatsFrame.Label = "24-hour Stats";
			coinStatsFrame.Add(coinStatsLabel);
			hbox1.PackStart(coinStatsFrame, false, false, 10);

            // Display blocks
            string prevblockHash = blockHash.blocks[0].hash;
            DateTime unixTimeBase = new DateTime(1970, 1, 1, 0, 0, 0);

            Table blockStatsTable = new Table(3, 6, false);
            blockStatsTable.SetColSpacing(0, 15);
            blockStatsTable.SetColSpacing(1, 15);

            Frame blockStatsFrame = new Frame();
            blockStatsFrame.Add(blockStatsTable);
            hbox2.PackStart(blockStatsFrame, false, false, 10);

            Label blockStatsLabel1 = new Label();
            blockStatsLabel1.Text = "Hash";
            Label blockStatsLabel2 = new Label();
            blockStatsLabel2.Text = "Height";
            Label blockStatsLabel3 = new Label();
            blockStatsLabel3.Text = "Time";
            blockStatsTable.Attach(blockStatsLabel1, 0, 1, 0, 1);
            blockStatsTable.Attach(blockStatsLabel2, 1, 2, 0, 1);
            blockStatsTable.Attach(blockStatsLabel3, 2, 3, 0, 1);

            List<Label> blockHashLabel = new List<Label>(){new Label(), new Label(), new Label(), new Label(), new Label()};
            List<Label> blockHeightLabel = new List<Label>() { new Label(), new Label(), new Label(), new Label(), new Label() };
            List<Label> blockTimeLabel = new List<Label>() { new Label(), new Label(), new Label(), new Label(), new Label() };
            for (int i = 0; i < 5; i++) {
                blockHashLabel[i].Text = blockHash.blocks[i].hash;
                blockStatsTable.Attach(blockHashLabel[i], 0, 1, 1+(uint)i, 2+(uint)i);

                blockHeightLabel[i].Text = blockHash.blocks[i].height.ToString();
                blockStatsTable.Attach(blockHeightLabel[i], 1, 2, 1 + (uint)i, 2 + (uint)i);

                DateTime blockHashTime = unixTimeBase.AddMilliseconds(blockHash.blocks[i].time);
                blockTimeLabel[i].Text = blockHashTime.ToString("MM/dd/yy H:mm");
                blockStatsTable.Attach(blockTimeLabel[i], 2, 3, 1 + (uint)i, 2 + (uint)i);
            }

            // Update stats
			uint timer = GLib.Timeout.Add(10000, new GLib.TimeoutHandler(() =>
			{
                // Coin stats
				coinStats = BitStats.GetCoinStats("BTC-USD");
				coinStatsChange = ((coinStats.last / coinStats.open) * 100 - 100);
				coinStatsLabelString = "Opening:\t\t" + coinStats.open.ToString("0.00") + 
				                                                 "\nCurrent:\t\t" + coinStats.last.ToString("0.00") + 
				                                                 "\nChange\t\t" + coinStatsChange.ToString("0.00") + "%" + (coinStatsChange > 0 ? "\u2191" : "\u2193");				coinStatsLabel.Text = coinStatsLabelString;

                // Blocks
				blockHash = BitStats.GetBlockStats();
                if (blockHash.blocks[0].hash == prevblockHash)
				{
                    prevblockHash = blockHash.blocks[0].hash;

                    for (int i = 0; i < 5; i++)
                    {
                        blockHashLabel[i].Text = blockHash.blocks[i].hash;
                        blockHeightLabel[i].Text = blockHash.blocks[i].height.ToString();
                        DateTime blockHashTime = unixTimeBase.AddMilliseconds(blockHash.blocks[i].time);
                        blockTimeLabel[i].Text = blockHashTime.ToString("MM/dd/yy H:mm");
                    }
				}

				return true;
			}));

			win.ShowAll();
			Application.Run();
		}
	}
}
