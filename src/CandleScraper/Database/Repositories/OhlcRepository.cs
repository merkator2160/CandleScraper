using CandleScraper.Database.Interfaces;
using CandleScraper.Database.Models.Filters;
using CandleScraper.Database.Models.Storage;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;

namespace CandleScraper.Database.Repositories
{
	public class OhlcRepository : MongoRepositoryBase<DailyOhlcDb>, IOhlcRepository
	{
		private readonly IMongoCollection<DailyOhlcDb> _collection;


		public OhlcRepository(IDataContext context) : base(context.Ohlcs)
		{
			_collection = context.Ohlcs;
		}


		// IOhlcRepository ////////////////////////////////////////////////////////////////////////
		public async Task<DailyOhlcDb[]> GetCryptoDailyOhlcFilteredAsync(GetCryptoDailyOhlcFilterDb filter)
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