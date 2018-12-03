using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	public class CmcpCryptocurrencyListingsLatestDataApi
	{
		[JsonProperty("id")]
		public Int64 Id { get; set; }

		[JsonProperty("name")]
		public String Name { get; set; }

		[JsonProperty("symbol")]
		public String Symbol { get; set; }

		[JsonProperty("slug")]
		public String Slug { get; set; }

		[JsonProperty("cmc_rank")]
		public Int64? CmcRank { get; set; }

		[JsonProperty("num_market_pairs")]
		public Int64 NumMarketPairs { get; set; }

		[JsonProperty("circulating_supply")]
		public Double? CirculatingSupply { get; set; }

		[JsonProperty("total_supply")]
		public Double? TotalSupply { get; set; }

		[JsonProperty("max_supply")]
		public Double? MaxSupply { get; set; }

		[JsonProperty("last_updated")]
		public DateTimeOffset LastUpdated { get; set; }

		[JsonProperty("date_added")]
		public DateTimeOffset DateAdded { get; set; }

		/// <summary>
		///  A map of market quotes in different currency conversions.The default map included is USD.
		/// </summary>
		[JsonProperty("quote")]
		public Dictionary<String, CmcpQuoteApi> Quote { get; set; }
	}
}