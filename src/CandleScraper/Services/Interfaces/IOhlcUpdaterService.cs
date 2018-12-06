using CandleScraper.Database.Models.Storage;
using CandleScraper.Services.Models;
using System;
using System.Threading.Tasks;

namespace CandleScraper.Services.Interfaces
{
	public interface IOhlcUpdaterService
	{
		DailyOhlcDto[] ScrapeForAsset(String startDate, String endDate, AssetDb asset);
		Task UpdateDatabaseDailyOhlcAsync(DailyOhlcDto[] ohlcsDto);
	}
}