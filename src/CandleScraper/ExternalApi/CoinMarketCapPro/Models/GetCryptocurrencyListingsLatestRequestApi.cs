using CandleScraper.ExternalApi.CoinMarketCapPro.Enums;
using System;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	public class GetCryptocurrencyListingsLatestRequestApi
	{
		public GetCryptocurrencyListingsLatestRequestApi()
		{
			StartPage = 1;
			ReturnLimit = 100;
			Convert = "USD";
			Sort = CryptocurrencySortApi.MarketCap;
			SortDir = SortDirectionApi.Asc;
			Type = CryptocurrencyTypeApi.All;
		}


		// PROPERTIES /////////////////////////////////////////////////////////////////////////////
		public String Sort { get; set; }
		public String SortDir { get; set; }
		public String Type { get; set; }
		public String Convert { get; set; }

		/// <summary>
		/// Optionally offset the start (1-based index) of the paginated list of items to return.
		/// </summary>
		public Int32 StartPage { get; set; }

		/// <summary>
		/// Optionally specify the number of results to return. Use this parameter and the "start" parameter to determine your own pagination size. [1, 5000]
		/// </summary>
		public Int32 ReturnLimit { get; set; }
	}
}