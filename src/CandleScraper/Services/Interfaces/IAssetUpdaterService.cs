using CandleScraper.ExternalApi.CoinMarketCapPro.Models;
using CandleScraper.Services.Models;
using System;
using System.Threading.Tasks;

namespace CandleScraper.Services.Interfaces
{
	public interface IAssetUpdaterService
	{
		Task<CmcpCryptocurrencyMapDataApi[]> CollectCryptoCurrencyMapAsync();
		Task<AssetDto[]> CollectCryptoCurrencyAssetInfoAsync(Int64[] cryptoMapIds);
		Task AddOrUpdateDatabaseAssetsAsync(AssetDto[] assets);
	}
}