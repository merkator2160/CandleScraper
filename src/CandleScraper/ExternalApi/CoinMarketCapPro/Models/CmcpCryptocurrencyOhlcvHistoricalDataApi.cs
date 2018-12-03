using Newtonsoft.Json;
using System;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	public class CmcpCryptocurrencyOhlcvHistoricalDataApi
	{
		[JsonProperty("id")]
		public Int64 Id { get; set; }

		[JsonProperty("name")]
		public String Name { get; set; }

		[JsonProperty("symbol")]
		public String Symbol { get; set; }

		[JsonProperty("quotes")]
		public QuoteElementApi[] Quotes { get; set; }
	}
}