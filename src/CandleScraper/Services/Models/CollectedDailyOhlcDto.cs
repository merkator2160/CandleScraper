using System;

namespace CandleScraper.Services.Models
{
	public class CollectedDailyOhlcDto
	{
		public Boolean IsProperlyCollected => CoinSummary.IsProperlyCollected && HistoricalData.IsProperlyCollected && AssetId != 0;

		public Int64 AssetId { get; set; }
		public String AssetName { get; set; }

		// Collecting on step 1
		public CoinSummaryDto CoinSummary { get; set; }

		// Collecting on step 2
		public HistoricalDataDto HistoricalData { get; set; }
	}
}