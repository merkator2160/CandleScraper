using System;

namespace CandleScraper.Services.Models
{
	public class HistoricalDataDto
	{
		public Boolean IsProperlyCollected => TimeOpen.HasValue &&
											  Open.HasValue &&
											  High.HasValue &&
											  Low.HasValue &&
											  Close.HasValue &&
											  Volume.HasValue &&
											  MarketCap.HasValue;

		public Int64? TimeOpen { get; set; }
		public Double? Open { get; set; }
		public Double? High { get; set; }
		public Double? Low { get; set; }
		public Double? Close { get; set; }
		public Double? Volume { get; set; }
		public Double? MarketCap { get; set; }
	}
}