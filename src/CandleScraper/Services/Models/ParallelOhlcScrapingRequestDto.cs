using CandleScraper.Database.Models.Storage;
using System;

namespace CandleScraper.Services.Models
{
	public class ParallelOhlcScrapingRequestDto
	{
		public AssetDb Asset { get; set; }
		public String StartDate { get; set; }
		public String EndDate { get; set; }
		public Int64 SequenceNumber { get; set; }
		public Int64 TotalNumber { get; set; }
	}
}