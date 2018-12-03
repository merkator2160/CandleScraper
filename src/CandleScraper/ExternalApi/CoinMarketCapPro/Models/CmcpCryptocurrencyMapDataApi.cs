using Newtonsoft.Json;
using System;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	public class CmcpCryptocurrencyMapDataApi
	{
		[JsonProperty("id")]
		public Int64 Id { get; set; }

		[JsonProperty("name")]
		public String Name { get; set; }

		[JsonProperty("symbol")]
		public String Symbol { get; set; }

		[JsonProperty("slug")]
		public String Slug { get; set; }

		[JsonProperty("is_active")]
		public Int64 IsActive { get; set; }

		[JsonProperty("first_historical_data")]
		public DateTimeOffset? FirstHistoricalData { get; set; }

		[JsonProperty("last_historical_data")]
		public DateTimeOffset? LastHistoricalData { get; set; }
	}
}