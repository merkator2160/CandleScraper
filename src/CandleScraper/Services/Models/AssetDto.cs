using System;

namespace CandleScraper.Services.Models
{
	public class AssetDto
	{
		public String Id { get; set; }
		public Int64 CoinMarketCapAssetId { get; set; }
		public String Currency { get; set; }
		public String Name { get; set; }
		public String Slug { get; set; }
		public String LogoUrl { get; set; }
		public String Category { get; set; }
	}
}