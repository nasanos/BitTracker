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

public class BlockHash
{
	public string hash { get; set; }
	public int time { get; set; }
	public int block_index { get; set; }
	public int height { get; set; }
	public int[] txIndexes { get; set; }
}

class BitStats
{
	public static CoinStats GetCoinStats(string product)
	{
		string base_url = "https://api.gdax.com/";

		HttpWebRequest stats_request = (HttpWebRequest) WebRequest.Create(base_url + "products/" + product + "/stats");
		stats_request.Method = WebRequestMethods.Http.Get;
		stats_request.ContentType = "application/json; charset=utf-8";
		stats_request.UserAgent = "Test Blockchain Stats Application";

		HttpWebResponse stats_response = stats_request.GetResponse() as HttpWebResponse;

		using (Stream responseStream = stats_response.GetResponseStream())
		{
			StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
			CoinStats stats = JsonConvert.DeserializeObject<CoinStats>(reader.ReadToEnd());

			return stats;
		}
	}

	public static BlockHash GetBlockStats()
	{
		string base_url = "https://blockchain.info/latestblock";

		HttpWebRequest block_request = (HttpWebRequest)WebRequest.Create(base_url);
		block_request.Method = WebRequestMethods.Http.Get;
		block_request.ContentType = "application/json; charset=utf-8";
		block_request.UserAgent = "Test Blockchain Stats Application";

		HttpWebResponse block_response = block_request.GetResponse() as HttpWebResponse;

		using (Stream responseStream = block_response.GetResponseStream())
		{
			StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
			BlockHash blockHash = JsonConvert.DeserializeObject<BlockHash>(reader.ReadToEnd());

			return blockHash;
		}
	}
}
