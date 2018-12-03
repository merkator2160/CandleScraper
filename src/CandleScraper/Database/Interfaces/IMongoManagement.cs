using MongoDB.Driver;

namespace CandleScraper.Database.Interfaces
{
	public interface IMongoManagement
	{
		IMongoClient Server { get; }
		IMongoDatabase Database { get; }
	}
}