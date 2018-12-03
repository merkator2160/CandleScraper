using System;

namespace CandleScraper.Services.Models
{
	public class CoinSummaryDto
	{
		public Boolean IsProperlyCollected => CirculatingSupply.HasValue;

		public Double? CirculatingSupply { get; set; }
		public Double? TotalSupply { get; set; }
		public Double? MaxSupply { get; set; }
	}
}