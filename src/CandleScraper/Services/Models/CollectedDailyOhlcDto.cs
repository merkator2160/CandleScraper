using MongoDB.Bson;
using System;

namespace CandleScraper.Services.Models
{
	public class CollectedDailyOhlcDto
	{
		public Boolean IsProperlyCollected => HistoricalData.IsProperlyCollected && AssetId != ObjectId.Empty;

		public ObjectId AssetId { get; set; }
		public String AssetName { get; set; }
		public CoinSummaryDto CoinSummary { get; set; }
		public HistoricalDataDto HistoricalData { get; set; }
	}
}