using Newtonsoft.Json;
using System;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	public class CmcpQuoteApi
	{
		[JsonProperty("price")]
		public Double? Price { get; set; }

		[JsonProperty("volume_24h")]
		public Int64? Volume24H { get; set; }

		[JsonProperty("percent_change_1h")]
		public Double? PercentChange1H { get; set; }

		[JsonProperty("percent_change_24h")]
		public Double? PercentChange24H { get; set; }

		[JsonProperty("percent_change_7d")]
		public Double? PercentChange7D { get; set; }

		[JsonProperty("market_cap")]
		public Double MarketCap { get; set; }

		[JsonProperty("last_updated")]
		public DateTimeOffset? LastUpdated { get; set; }
	}
}
