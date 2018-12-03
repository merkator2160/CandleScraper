using CandleScraper.Database.Interfaces;
using CandleScraper.Database.Models.Storage;
using MongoDB.Driver;

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
	}
}