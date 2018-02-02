using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

public class CoinStats
{
	public float open { get; set; }
	public float high { get; set; }
	public float low { get; set; }
	public float volume { get; set; }
	public float last { get; set; }
	public float volume_30day { get; set; }
}

public class Block
{
    public int height { get; set; }
	public string hash { get; set; }
	public long time { get; set; }
}

public class BlockList
{
    public Block[] blocks { get; set; }
}

class BitStats
{
	public static CoinStats GetCoinStats(string product)
	{
		string baseUrl = "https://api.gdax.com/";

		HttpWebRequest statsRequest = (HttpWebRequest) WebRequest.Create(baseUrl + "products/" + product + "/stats");
		statsRequest.Method = WebRequestMethods.Http.Get;
		statsRequest.ContentType = "application/json; charset=utf-8";
		statsRequest.UserAgent = "Test Blockchain Stats Application";

		HttpWebResponse statsResponse = statsRequest.GetResponse() as HttpWebResponse;

		using (Stream responseStream = statsResponse.GetResponseStream())
		{
			StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
			CoinStats stats = JsonConvert.DeserializeObject<CoinStats>(reader.ReadToEnd());

			return stats;
		}
	}

	public static BlockList GetBlockStats()
	{
        string baseUrl = "https://blockchain.info/blocks/";
        long milliTime = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;

        HttpWebRequest blockRequest = (HttpWebRequest)WebRequest.Create(baseUrl + milliTime + "?format=json");
		blockRequest.Method = WebRequestMethods.Http.Get;
		blockRequest.ContentType = "application/json; charset=utf-8";
        blockRequest.UserAgent = "Test Blockchain Stats Application";

        HttpWebResponse blockResponse = blockRequest.GetResponse() as HttpWebResponse;

		using (Stream responseStream = blockResponse.GetResponseStream())
		{
			StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
			BlockList blockHash = JsonConvert.DeserializeObject<BlockList>(reader.ReadToEnd());

			return blockHash;
		}
	}
}
