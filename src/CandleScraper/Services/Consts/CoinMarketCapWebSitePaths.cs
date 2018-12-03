using System;

namespace CandleScraper.Core.Consts
{
	public static class CoinMarketCapPaths
	{
		// Coin summary //
		public const String CirculatingSupply = @"/html/body/div[@class='container main-section']/div[@class='row']/div[@class='col-xl-10 padding-top-1x']/div[@class='details-panel flex-container bottom-margin-2x']/div[@class='details-panel-item--marketcap-stats flex-container']/div[@class='coin-summary-item'][3]/div[@class='coin-summary-item-detail details-text-medium']/span";
		public const String MaxSupplyOrTotalSupplyHeader = @"/html/body/div[@class='container main-section']/div[@class='row']/div[@class='col-xl-10 padding-top-1x']/div[@class='details-panel flex-container bottom-margin-2x']/div[@class='details-panel-item--marketcap-stats flex-container']/div[@class='coin-summary-item'][4]/h5[@class='coin-summary-item-header']";
		public const String MaxSupplyOrTotalSupplyValue = @"/html/body/div[@class='container main-section']/div[@class='row']/div[@class='col-xl-10 padding-top-1x']/div[@class='details-panel flex-container bottom-margin-2x']/div[@class='details-panel-item--marketcap-stats flex-container']/div[@class='coin-summary-item'][4]/div[@class='coin-summary-item-detail details-text-medium']/span";

		// Historical data //
		public const String HistoricalDataTable = @"/html/body/div[@class='container main-section']/div[@class='row']/div[@class='col-xl-10 padding-top-1x']/div[@class='row bottom-margin-1x']/div[@class='col-xs-12 tab-content']/div[@id='historical-data']/div[@class='padding-top-1x']/div[@class='table-responsive']/table[@class='table']/tbody/tr[@class='text-right']";
	}
}