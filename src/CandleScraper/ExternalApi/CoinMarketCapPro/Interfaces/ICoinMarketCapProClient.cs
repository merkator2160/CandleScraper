using CandleScraper.ExternalApi.CoinMarketCapPro.Models;
using System;
using System.Threading.Tasks;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Interfaces
{
	public interface ICoinMarketCapProClient
	{
		Task<CmcpCryptocurrencyMapApi> GetCryptocurrencyMapAsync(GetCryptocurrencyMapRequestApi request);
		Task<CmcpCryptocurrencyInfoApi> GetCryptocurrencyInfoAsync(Int64[] id, String[] symbol);
		Task<CmcpCryptocurrencyListingsLatestApi> GetCryptocurrencyListingLatest(GetCryptocurrencyListingsLatestRequestApi request);
	}
}