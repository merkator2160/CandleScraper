using CandleScraper.Database.Interfaces;
using CandleScraper.Database.Models.Storage;
using MongoDB.Driver;

namespace CandleScraper.Database.Repositories
{
	public class OhlcRepository : MongoRepositoryBase<OhlcDb>, IOhlcRepository
	{
		private readonly IMongoCollection<OhlcDb> _collection;


		public OhlcRepository(IDataContext context) : base(context.Ohlcs)
		{
			_collection = context.Ohlcs;
		}


		// IOhlcRepository ////////////////////////////////////////////////////////////////////////
	}
}