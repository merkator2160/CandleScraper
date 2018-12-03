using Newtonsoft.Json;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	/// <summary>
	/// API response for /v1/cryptocurrency/listings/latest
	/// See https://pro.coinmarketcap.com/api/v1#operation/getV1CryptocurrencyListingsLatest
	/// </summary>
	public class CmcpCryptocurrencyListingsLatestApi
	{
		[JsonProperty("status")]
		public CmcpApiStatusApi Status { get; set; }

		[JsonProperty("data")]
		public CmcpCryptocurrencyListingsLatestDataApi[] Data { get; set; }
	}
}