using CandleScraper.Database.Interfaces;
using MongoDB.Bson;
using System;

namespace CandleScraper.Database.Models.Storage
{
	public class AssetDb : IStorageEntity
	{
		public ObjectId Id { get; set; }
		public Int64 CoinMarketCapAssetId { get; set; }
		public String Currency { get; set; }
		public String Name { get; set; }
		public String Slug { get; set; }
		public String LogoUrl { get; set; }
		public String Category { get; set; }
	}
}