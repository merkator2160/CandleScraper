using System;

namespace CandleScraper.Services.Models
{
	public class CoinSummaryDto
	{
		public Double? CirculatingSupply { get; set; }
		public Double? TotalSupply { get; set; }
		public Double? MaxSupply { get; set; }
	}
}