using MongoDB.Bson;
using System;

namespace CandleScraper.Database.Models.Filters
{
	public class StatDateFilterDb
	{
		public ObjectId AssetId { get; set; }
		public Int64 TimeOpenStart { get; set; }
		public Int64 TimeOpenEnd { get; set; }
	}
}