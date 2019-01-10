using CandleScraper.Database.Models.Config;
using CandleScraper.Database.Models.Storage;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Reflection;

namespace CandleScraper.Database
{
	public class DataContext
	{
		private readonly MongoDbConfig _config;


		static DataContext()
		{
			RegisterMappings();
		}
		public DataContext(IMongoClient mongoClient, MongoDbConfig config)
		{
			_config = config;
			Client = mongoClient;
			Database = Client.GetDatabase(config.DatabaseName);
		}


		// ENTITIES ///////////////////////////////////////////////////////////////////////////////
		public IMongoCollection<AssetDb> Assets => Database.GetCollection<AssetDb>("Assets");
		public IMongoCollection<DailyOhlcDb> Ohlcs => Database.GetCollection<DailyOhlcDb>("Ohlcs");


		// SERVER /////////////////////////////////////////////////////////////////////////////////
		public IMongoClient Client { get; }
		public IMongoDatabase Database { get; }


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		private static void RegisterMappings()
		{
			var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
				.Where(type => !String.IsNullOrEmpty(type.Namespace))
				.Where(type => IsSubclassOfRawGeneric(typeof(BsonClassMap<>), type)).ToArray()
				.ToArray();

			foreach(var x in typesToRegister)
			{
				dynamic configurationInstance = Activator.CreateInstance(x);
				BsonClassMap.RegisterClassMap(configurationInstance);
			}
		}
		private static Boolean IsSubclassOfRawGeneric(Type generic, Type toCheck)
		{
			while(toCheck != null && toCheck != typeof(Object))
			{
				var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
				if(generic == cur)
					return true;
				toCheck = toCheck.BaseType;
			}
			return false;
		}
	}
}