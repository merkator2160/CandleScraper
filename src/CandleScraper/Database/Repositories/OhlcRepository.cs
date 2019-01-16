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
		private readonly DataContext _context;


		public OhlcRepository(DataContext context) : base(context.Ohlcs)
		{
			_context = context;
		}


		// IOhlcRepository ////////////////////////////////////////////////////////////////////////
		public async Task<DailyOhlcDb[]> GetCryptoDailyOhlcFilteredAsync(GetCryptoDailyOhlcFilterDb filter)
		{
			return (await _context.Ohlcs.AsQueryable()
				.Where(p => p.AssetId.Equals(filter.AssetId)
							&& p.TimeOpen >= filter.TimeOpenStart
							&& p.TimeOpen < filter.TimeOpenEnd)
				.ToListAsync())
				.ToArray();
		}
	}
}