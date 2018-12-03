using CandleScraper.ExternalApi.CoinMarketCapPro.Enums;
using System;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	/// <summary>
	/// https://pro.coinmarketcap.com/api/v1#operation/getV1CryptocurrencyMap
	/// </summary>
	public class GetCryptocurrencyMapRequestApi
	{
		/// <summary>
		/// This uses default param that CoinMarketCap API has by default
		/// (except limit param, which is not specified by the API. We have to specify limit value due to C# requirement)
		/// </summary>
		public GetCryptocurrencyMapRequestApi()
		{
			ListingStatus = ListingStatusApi.Active;
			StartPage = 1;
			PageSize = 10;
		}


		// PROPERTIES /////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Only active coins are returned by default. Pass 'inactive' to get a list of coins that are no longer active.
		/// </summary>
		public String ListingStatus { get; set; }

		/// <summary>
		/// Optionally pass a list of cryptocurrency symbols to return CoinMarketCap IDs for. If this option is passed, other options will be ignored.
		/// </summary>
		public String[] Symbol { get; set; }

		/// <summary>
		/// Optionally offset the start (1-based index) of the paginated list of items to return.
		/// </summary>
		public Int32 StartPage { get; set; }

		/// <summary>
		/// Optionally specify the number of results to return. Use this parameter and the "start" parameter to determine your own pagination size. [1, 5000]
		/// </summary>
		public Int32 PageSize { get; set; }
	}
}