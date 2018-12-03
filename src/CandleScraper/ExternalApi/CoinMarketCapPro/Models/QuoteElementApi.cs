using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	public class QuoteElementApi
	{
		[JsonProperty("time_open")]
		public DateTimeOffset TimeOpen { get; set; }

		[JsonProperty("time_close")]
		public DateTimeOffset TimeClose { get; set; }

		/// <summary>
		///  A map of market quotes in different currency conversions.The default map included is USD.
		/// </summary>
		[JsonProperty("quote")]
		public Dictionary<String, CmcpOhlcvApi> Quote { get; set; }
	}
}