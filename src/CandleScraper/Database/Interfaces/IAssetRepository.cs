using CandleScraper.Database.Models.Storage;
using System;
using System.Threading.Tasks;

namespace CandleScraper.Database.Interfaces
{
	public interface IAssetRepository : IRepository<AssetDb>
	{
		Task<AssetDb> GetByCoinMarketCapAssetIdAsync(Int64 id);
	}
}