using CandleScraper.Database.Models.Storage;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace CandleScraper.Database.Mappings
{
	public class AssetMap : BsonClassMap<AssetDb>
	{
		public AssetMap()
		{
			AutoMap();
			MapIdMember(c => c.Id).SetIdGenerator(new ObjectIdGenerator());
			MapMember(p => p.Category).SetIgnoreIfNull(true);
		}
	}
}