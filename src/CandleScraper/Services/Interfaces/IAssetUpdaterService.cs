using CandleScraper.Database.Models.Storage;
using CandleScraper.ExternalApi.CoinMarketCapPro.Models;
using System;
using System.Threading.Tasks;

namespace CandleScraper.Services.Interfaces
{
	public interface IAssetUpdaterService
	{
		Task<CmcpCryptocurrencyMapDataApi[]> CollectCryptoCurrencyMapAsync();
		Task<AssetDb[]> CollectCryptoCurrencyAssetInfoAsync(Int64[] cryptoMapIds);
		Task AddOrUpdateDatabaseAssetsAsync(AssetDb[] assets);
	}
}