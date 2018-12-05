using CandleScraper.Database.Interfaces;
using MongoDB.Bson;
using System;

namespace CandleScraper.Database.Models.Storage
{
	public class OhlcDb : IStorageEntity
	{
		public ObjectId Id { get; set; }
		public ObjectId AssetId { get; set; }

		/// <summary>
		/// Opening price in USD
		/// </summary>
		public Int64 TimeOpen { get; set; }

		/// <summary>
		/// Opening price in USD
		/// </summary>
		public Double Open { get; set; }

		/// <summary>
		/// Highest price in USD
		/// </summary>
		public Double High { get; set; }

		/// <summary>
		/// Lowest price in USD
		/// </summary>
		public Double Low { get; set; }

		/// <summary>
		/// Closing price in USD
		/// </summary>
		public Double Close { get; set; }

		/// <summary>
		/// Total traded volume over daily period
		/// </summary>
		public Double Volume { get; set; }

		/// <summary>
		/// Market Cap in USD
		/// </summary>
		public Double MarketCap { get; set; }

		/// <summary>
		/// Amount of coins in curculation
		/// </summary>
		public Double? CirculatingSupply { get; set; }

		/// <summary>
		/// Amount of coins in supply. Not always the same as Circulating Supply
		/// </summary>
		public Double? TotalSupply { get; set; }

		/// <summary>
		/// Maximum possible coins that can exist.
		/// </summary>
		public Double? MaxSupply { get; set; }
	}
}
