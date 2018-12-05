using CandleScraper.Database.Models.Filters;
using CandleScraper.Database.Models.Storage;
using System.Threading.Tasks;

namespace CandleScraper.Database.Interfaces
{
	public interface IOhlcRepository : IRepository<OhlcDb>
	{
		Task<OhlcDb[]> GetCryptoDailyOhlcFilteredAsync(StatDateFilterDb filter);
	}
}