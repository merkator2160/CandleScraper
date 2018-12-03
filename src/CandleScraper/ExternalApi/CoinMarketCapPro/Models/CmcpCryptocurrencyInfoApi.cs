using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	/// <summary>
	/// API response for /v1/cryptocurrency/info.
	/// See https://pro.coinmarketcap.com/api/v1#operation/getV1CryptocurrencyInfo
	/// </summary>
	public class CmcpCryptocurrencyInfoApi
	{
		[JsonProperty("status")]
		public CmcpApiStatusApi Status { get; set; }

		/// <summary>
		/// /v1/cryptocurrency/info returns array of objects whose
		/// field name is same as CMCPCryptocurrencyInfoData.Id
		/// </summary>
		[JsonProperty("data")]
		public Dictionary<String, CmcpCryptocurrencyInfoDataApi> Data { get; set; }
	}
}
