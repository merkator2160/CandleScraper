using Newtonsoft.Json;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	/// <summary>
	/// API response for /v1/cryptocurrency/ohlcv/historical
	/// See https://pro.coinmarketcap.com/api/v1#operation/getV1CryptocurrencyOhlcvHistorical
	/// </summary>
	public class CmcpCryptocurrencyOhlcvHistoricalApi
	{
		[JsonProperty("status")]
		public CmcpApiStatusApi Status { get; set; }

		[JsonProperty("data")]
		public CmcpCryptocurrencyOhlcvHistoricalDataApi[] Data { get; set; }
	}
}
