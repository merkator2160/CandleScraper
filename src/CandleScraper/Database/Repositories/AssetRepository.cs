using CandleScraper.Database.Interfaces;
using CandleScraper.Database.Models.Storage;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace CandleScraper.Database.Repositories
{
	public class AssetRepository : MongoRepositoryBase<AssetDb>, IAssetRepository
	{
		private readonly IMongoCollection<AssetDb> _collection;


		public AssetRepository(IDataContext context) : base(context.Assets)
		{
			_collection = context.Assets;
		}


		// IAssetRepository ///////////////////////////////////////////////////////////////////////
		public async Task<AssetDb> GetByCoinMarketCapAssetIdAsync(Int64 id)
		{
			return await (await _collection.FindAsync(p => p.CoinMarketCapAssetId == id)).FirstOrDefaultAsync();
		}
	}
}