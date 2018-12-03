using Newtonsoft.Json;
using System;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	public class CmcpCryptocurrencyInfoDataApi
	{
		[JsonProperty("id")]
		public Int64 Id { get; set; }

		[JsonProperty("urls")]
		public UrlsApi Urls { get; set; }

		[JsonProperty("name")]
		public String Name { get; set; }

		[JsonProperty("symbol")]
		public String Symbol { get; set; }

		[JsonProperty("slug")]
		public String Slug { get; set; }

		[JsonProperty("logo")]
		public String Logo { get; set; }

		[JsonProperty("date_added")]
		public DateTimeOffset? DateAdded { get; set; }

		[JsonProperty("tags")]
		public String[] Tags { get; set; }

		[JsonProperty("category")]
		public String Category { get; set; }
	}
}