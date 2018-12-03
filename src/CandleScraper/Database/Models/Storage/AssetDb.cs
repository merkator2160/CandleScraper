using CandleScraper.Database.Interfaces;
using MongoDB.Bson;
using System;

namespace CandleScraper.Database.Models.Storage
{
	public class AssetDb : IStorageEntity
	{
		public ObjectId Id { get; set; }

		///<summary>
		/// Asset Currency symbol, like "BTC". Follows global standard as much as possible.
		/// See exchange-specific mapping tables for incongruity
		///</summary>
		public String Currency { get; set; }

		///<summary>
		/// Asset descriptive name, like "Bitcoin"
		///</summary>
		public String Name { get; set; }

		/// <summary>
		/// Asset name slug.
		/// </summary>
		public String Slug { get; set; }

		/// <summary>
		/// Coin logo URL. Max 2048 char b/c of Internet Explorer
		/// </summary>
		public String LogoUrl { get; set; }

		/// <summary>
		/// Whether this cryptocurrency is still actively being traded on any of the exchanges.
		/// </summary>
		public Boolean IsActivelyTraded { get; set; }

		/// <summary>
		/// Consensus Proof Type look like "PoW" or "PoW/PoS" or "Other"
		/// </summary>
		public String ProofType { get; set; }

		/// <summary>
		/// Token Standard, e.g. ERC-20, if applicable
		/// </summary>
		public String TokenStandard { get; set; }

		/// <summary>
		/// Cryptocurrency category (defined by Panda team)
		/// </summary>
		public String Category { get; set; }

		/// <summary>
		/// Cryptocurrency Industry Sector (defined by Panda team)
		/// </summary>
		public String IndustrySector { get; set; }

		/// <summary>
		/// Coin's Algorithm used for Proof
		/// </summary>
		public String Algorithm { get; set; }
	}
}