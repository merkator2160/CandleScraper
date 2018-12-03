using Newtonsoft.Json;
using System;

namespace CandleScraper.ExternalApi.CoinMarketCapPro.Models
{
	public class CmcpAvailableSupplyApi
	{
		[JsonProperty("market_cap_by_available_supply")]
		public Decimal[] MarketCapByAvailableSupply { get; set; }

		[JsonProperty("price_usd")]
		public Decimal[] PriceUsd { get; set; }
	}


}
