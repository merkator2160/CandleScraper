using CandleScraper.Database.Interfaces;
using CandleScraper.Database.Models.Filters;
using CandleScraper.Database.Models.Storage;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;

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
		public async Task<OhlcDb[]> GetCryptoDailyOhlcFilteredAsync(StatDateFilterDb filter)
		{
			return (await _collection.AsQueryable()
				.Where(p => p.AssetId.Equals(filter.AssetId)
							&& p.TimeOpen >= filter.TimeOpenStart
							&& p.TimeOpen < filter.TimeOpenEnd)
				.ToListAsync())
				.ToArray();
		}
	}
}