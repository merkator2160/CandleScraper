using CandleScraper.Database.Models.Storage;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace CandleScraper.Database.Mappings
{
	public class OhlcMap : BsonClassMap<DailyOhlcDb>
	{
		public OhlcMap()
		{
			AutoMap();
			MapIdMember(c => c.Id).SetIdGenerator(new ObjectIdGenerator());
			MapMember(c => c.CirculatingSupply).SetIgnoreIfNull(true);
			MapMember(c => c.TotalSupply).SetIgnoreIfNull(true);
			MapMember(c => c.MaxSupply).SetIgnoreIfNull(true);
		}
	}
}