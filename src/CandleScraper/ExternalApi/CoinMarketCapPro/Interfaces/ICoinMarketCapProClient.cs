using CandleScraper.ExternalApi.CoinMarketCapPro.Models;
using System;
using System.Threading.Tasks;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Interfaces
{
	public interface ICoinMarketCapProClient
	{
		Task<CmcpAvailableSupplyApi> GetSupplyData(String slug, Int64 start, Int64 end);
		Task<CmcpCryptocurrencyMapApi> GetCryptoCurrencyMapAsync(GetCryptocurrencyMapRequestApi request);
		Task<CmcpCryptocurrencyInfoApi> GetCryptoCurrencyInfoAsync(Int64[] id);
		Task<CmcpCryptocurrencyInfoApi> GetCryptoCurrencyInfoAsync(Int64[] id, String[] symbol);
		Task<CmcpCryptocurrencyListingsLatestApi> GetCryptoCurrencyListingLatest(GetCryptocurrencyListingsLatestRequestApi request);
	}
}