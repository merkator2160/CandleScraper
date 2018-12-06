using CandleScraper.Database.Models.Storage;
using MongoDB.Driver;

namespace CandleScraper.Database.Interfaces
{
	public interface IDataContext
	{
		IMongoCollection<AssetDb> Assets { get; }
		IMongoCollection<DailyOhlcDb> Ohlcs { get; }
	}
}