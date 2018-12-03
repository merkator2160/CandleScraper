using Newtonsoft.Json;
using System;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	public class CmcpOhlcvApi
	{
		[JsonProperty("open")]
		public Double Open { get; set; }

		[JsonProperty("high")]
		public Double High { get; set; }

		[JsonProperty("low")]
		public Double Low { get; set; }

		[JsonProperty("close")]
		public Double Close { get; set; }

		[JsonProperty("volume")]
		public Double Volume { get; set; }

		[JsonProperty("timestamp")]
		public DateTimeOffset Timestamp { get; set; }
	}
}
