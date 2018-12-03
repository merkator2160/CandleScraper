using Newtonsoft.Json;
using System;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	/// <summary>
	/// Base response object for Coin Market Cap Pro API V1.
	/// See https://pro.coinmarketcap.com/api/v1#section/Standards-and-Conventions
	/// </summary>
	public class CmcpApiStatusApi
	{
		[JsonProperty("timestamp")]
		public DateTimeOffset Timestamp { get; set; }

		[JsonProperty("error_code")]
		public Int64 ErrorCode { get; set; }

		[JsonProperty("error_message")]
		public Object ErrorMessage { get; set; }

		[JsonProperty("elapsed")]
		public Int64 Elapsed { get; set; }

		[JsonProperty("credit_count")]
		public Int64 CreditCount { get; set; }
	}
}
