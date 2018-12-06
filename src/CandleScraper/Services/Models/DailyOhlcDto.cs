using System;

namespace CandleScraper.Services.Models
{
	public class DailyOhlcDto
	{
		public String Id { get; set; }
		public String AssetId { get; set; }
		public String AssetName { get; set; }
		public Int64 TimeOpen { get; set; }
		public Double Open { get; set; }
		public Double High { get; set; }
		public Double Low { get; set; }
		public Double Close { get; set; }
		public Double Volume { get; set; }
		public Double MarketCap { get; set; }
		public Double? CirculatingSupply { get; set; }
		public Double? TotalSupply { get; set; }
		public Double? MaxSupply { get; set; }
	}
}