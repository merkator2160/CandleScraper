using CandleScraper.Database.Interfaces;
using CandleScraper.Database.Models.Config;
using CandleScraper.Database.Models.Storage;
using MongoDB.Driver;

namespace CandleScraper.Database
{
	public class DataContext : IDataContext, IMongoManagement
	{
		private readonly MongoDbConfig _config;


		public DataContext(IMongoClient mongoServerClient, MongoDbConfig config)
		{
			_config = config;
			Server = mongoServerClient;
			Database = Server.GetDatabase(config.DatabaseName);
		}


		// IDataContext //////////////////////////////////////////////////////////////////////
		public IMongoCollection<AssetDb> Assets => Database.GetCollection<AssetDb>("Assets");
		public IMongoCollection<OhlcDb> Ohlcs => Database.GetCollection<OhlcDb>("Ohlcs");


		// IMongoManagement ///////////////////////////////////////////////////////////////////////
		public IMongoClient Server { get; }
		public IMongoDatabase Database { get; }
	}
}