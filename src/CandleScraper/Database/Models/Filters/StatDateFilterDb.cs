using System;
using System.ComponentModel.DataAnnotations;

namespace CandleScraper.Database.Models.Filters
{
	public class StatDateFilterDb
	{
		/// <summary>
		/// Crypto Asset Id filter
		/// </summary>
		[Required]
		public Int64 AssetId { get; set; }

		/// <summary>
		/// starting day's datetime for 12:00:00 AM of the day
		/// </summary>
		[Required]
		public Int64 TimeOpenStart { get; set; }

		/// <summary>
		/// ending day's datetime for 12:00:00 AM of the day 
		/// </summary>
		[Required]
		public Int64 TimeOpenEnd { get; set; }
	}
}
