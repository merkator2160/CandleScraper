using CandleScraper.Database.Models.Storage;
using System.Threading.Tasks;

namespace CandleScraper.Services.Interfaces
{
	public interface IAssetUpdaterService
	{
		Task<AssetDb[]> GetAssetsFromMarketAsync();
		Task UpdateDatabaseAssetsAsync(AssetDb[] assets);
	}
}