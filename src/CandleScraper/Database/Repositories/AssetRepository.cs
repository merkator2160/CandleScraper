using CandleScraper.Database.Interfaces;
using CandleScraper.Database.Models.Storage;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace CandleScraper.Database.Repositories
{
	public class AssetRepository : MongoRepositoryBase<AssetDb>, IAssetRepository
	{
		private readonly DataContext _context;


		public AssetRepository(DataContext context) : base(context.Assets)
		{
			_context = context;
		}


		// IAssetRepository ///////////////////////////////////////////////////////////////////////
		public async Task<AssetDb> GetByCoinMarketCapAssetIdAsync(Int64 id)
		{
			using(var cursor = await _context.Assets.FindAsync(p => p.CoinMarketCapAssetId == id))
			{
				return await cursor.FirstOrDefaultAsync();
			}
		}
	}
}