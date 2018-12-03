using MongoDB.Bson;

namespace CandleScraper.Database.Interfaces
{
	public interface IStorageEntity
	{
		ObjectId Id { get; set; }
	}
}