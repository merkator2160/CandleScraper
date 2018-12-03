using CandleScraper.ExternalApi.CoinMarketCapPro.Enums;
using Newtonsoft.Json;
using System;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	/// <summary>
	/// https://pro.coinmarketcap.com/api/v1#operation/getV1CryptocurrencyOhlcvHistorical
	/// </summary>
	public class GetCryptocurrencyOhlcvHistoricalRequestApi
	{
		public GetCryptocurrencyOhlcvHistoricalRequestApi()
		{
			TimePeriod = "daily";
			Count = 10;
			Interval = CryptocurrencyIntervalOptionApi.Daily;
			Convert = "USD";
		}


		// PROPERTIES /////////////////////////////////////////////////////////////////////////////
		public Int64 Id { get; set; }
		public String Symbol { get; set; }

		//[JsonProperty("time_period")]
		//public DateTimeOffset TimePeriod { get; set; }
		//[JsonProperty("time_start")]
		//public DateTimeOffset TimeStart { get; set; }
		//[JsonProperty("time_end")]
		//public DateTimeOffset TimeEnd { get; set; }

		// CoinMarketCap Pro API says "Unix or ISO 8601". We should use Unix for consistency
		// Check if this Unix timestamp is seconds or miliseconds
		[JsonProperty("time_period")]
		public String TimePeriod { get; set; }

		[JsonProperty("time_start")]
		public Int64 TimeStart { get; set; }
		[JsonProperty("time_end")]
		public Int64 TimeEnd { get; set; }



		public Int32 Count { get; set; }
		public String Interval { get; set; }
		public String Convert { get; set; }
	}
}