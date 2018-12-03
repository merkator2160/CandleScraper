using Newtonsoft.Json;
using System;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	public class UrlsApi
	{
		[JsonProperty("website")]
		public String[] Website { get; set; }

		[JsonProperty("twitter")]
		public String[] Twitter { get; set; }

		[JsonProperty("reddit")]
		public String[] Reddit { get; set; }

		[JsonProperty("message_board")]
		public String[] MessageBoard { get; set; }

		[JsonProperty("announcement")]
		public String[] Announcement { get; set; }

		[JsonProperty("chat")]
		public String[] Chat { get; set; }

		[JsonProperty("explorer")]
		public String[] Explorer { get; set; }

		[JsonProperty("source_code")]
		public String[] SourceCode { get; set; }
	}
}