using Newtonsoft.Json;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	/// <summary>
	/// API response for /v1/cryptocurrency/map.
	/// See https://pro.coinmarketcap.com/api/v1#operation/getV1CryptocurrencyMap
	/// </summary>
	public class CmcpCryptocurrencyMapApi : CmcpApiStatusApi
	{
		[JsonProperty("status")]
		public CmcpApiStatusApi Status { get; set; }

		[JsonProperty("data")]
		public CmcpCryptocurrencyMapDataApi[] Data { get; set; }
	}
}
