using CandleScraper.Core.Consts;
using CandleScraper.Core.Helpers;
using CandleScraper.Database.Interfaces;
using CandleScraper.Database.Models.Filters;
using CandleScraper.Database.Models.Storage;
using CandleScraper.Services.Interfaces;
using CandleScraper.Services.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CandleScraper.Services
{
	public class OhlcUpdaterService : IOhlcUpdaterService
	{
		private readonly IRepositoryBundle _unitOfWork;


		public OhlcUpdaterService(IRepositoryBundle unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		// IOhlcUpdaterService ////////////////////////////////////////////////////////////////////
		public CollectedDailyOhlcDto[] ScrapeForAsset(String startDate, String endDate, AssetDb asset)
		{
			var newCryptoDailyOhlcDbList = new List<CollectedDailyOhlcDto>();

			if(!TryGetPage($"https://coinmarketcap.com/currencies/{asset.Slug}/historical-data/?start={startDate}&end={endDate}", out var htmlDoc))
				return newCryptoDailyOhlcDbList.ToArray();

			var coinSummary = CollectCoinSummary(htmlDoc);
			var historicalData = CollectHistoricalData(htmlDoc);

			foreach(var x in historicalData)
			{
				newCryptoDailyOhlcDbList.Add(new CollectedDailyOhlcDto()
				{
					AssetId = asset.Id,
					AssetName = asset.Name,
					CoinSummary = coinSummary,
					HistoricalData = x
				});
			}

			return newCryptoDailyOhlcDbList.ToArray();
		}
		public async Task UpdateDatabaseDailyOhlcAsync(CollectedDailyOhlcDto[] ohlcsDto)
		{
			var newOrderedCryptoDailyOhlcDb = ohlcsDto.OrderBy(p => p.HistoricalData.TimeOpen).ToArray();

			var workingStep = 100;
			for(var i = 0; i < newOrderedCryptoDailyOhlcDb.Length; i = i + workingStep)
			{
				var cryptoOhlcs = newOrderedCryptoDailyOhlcDb.Skip(i).Take(workingStep).ToArray();
				foreach(var x in cryptoOhlcs)
				{
					if(!x.IsProperlyCollected)
						continue;

					var existingOhlc = await _unitOfWork.Ohlcs.GetCryptoDailyOhlcFilteredAsync(new StatDateFilterDb()
					{
						AssetId = x.AssetId,
						TimeOpenStart = x.HistoricalData.TimeOpen.Value,
						TimeOpenEnd = x.HistoricalData.TimeOpen.Value + 1.DaysToMs() - 1
					});

					if(existingOhlc != null && existingOhlc.Length == 0)
					{
						await _unitOfWork.Ohlcs.AddAsync(new OhlcDb()
						{
							AssetId = x.AssetId,
							TimeOpen = x.HistoricalData.TimeOpen.Value,
							Open = x.HistoricalData.Open.Value,
							High = x.HistoricalData.High.Value,
							Low = x.HistoricalData.Low.Value,
							Close = x.HistoricalData.Close.Value,
							Volume = x.HistoricalData.Volume.Value,
							MarketCap = x.HistoricalData.MarketCap.Value,
							CirculatingSupply = x.CoinSummary.CirculatingSupply,
							TotalSupply = x.CoinSummary.TotalSupply,
							MaxSupply = x.CoinSummary.MaxSupply
						});
#if DEBUG
						Console.WriteLine($"{DateTime.Now} | Added new OHLC for Currency: {x.AssetName} and TimeOpen: {x.HistoricalData.TimeOpen.Value.FromPosixTimeMs():G}");
#endif
					}
				}
			}
		}


		// SUPPORT FUNCTIONS //////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Step1:
		/// Save coin summary such as Market cap, volume 24h, Circulating Supply, Total, Supply, and Max Supply from coin summary item list.
		/// This information is a snapshot at the time of scraping. It is likely not possible to scrape all coins exactly at midnight UTC, but 
		/// It is acceptable to store Circulating Supply, Tota, Supply, and Max Supply at not-exact midnight date.
		/// Market Cap and Volume changes more frequently, so we will collect this data in Step 2 using historical data table.
		/// </summary>
		private CoinSummaryDto CollectCoinSummary(HtmlDocument htmlDoc)
		{
			return new CoinSummaryDto()
			{
				CirculatingSupply = GetCirculatingSupply(htmlDoc),
				MaxSupply = GetMaxSupply(htmlDoc),
				TotalSupply = GetTotalSupply(htmlDoc)
			};
		}
		private Double? GetCirculatingSupply(HtmlDocument htmlDoc)
		{
			try
			{
				var circulatingSupplyStr = htmlDoc.DocumentNode.SelectSingleNode(CoinMarketCapPaths.CirculatingSupply).InnerText.Replace("\n", "");
				if(Double.TryParse(circulatingSupplyStr, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var circulatingSupply))
				{
					return circulatingSupply;
				}

				return null;
			}
			catch(NullReferenceException)
			{
				return null;
			}
		}
		private Double? GetMaxSupply(HtmlDocument htmlDoc)
		{
			try
			{
				var maxSupplyOrTotalSupplyHeader = htmlDoc.DocumentNode.SelectSingleNode(CoinMarketCapPaths.MaxSupplyOrTotalSupplyHeader).InnerText.Replace("\n", "");
				var maxSupplyOrTotalSupplyValueStr = htmlDoc.DocumentNode.SelectSingleNode(CoinMarketCapPaths.MaxSupplyOrTotalSupplyValue).InnerText.Replace("\n", "");
				if(Double.TryParse(maxSupplyOrTotalSupplyValueStr, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var maxSupplyOrTotalSupplyValue))
				{
					if(maxSupplyOrTotalSupplyHeader.Equals("Max Supply"))
					{
						return maxSupplyOrTotalSupplyValue;
					}
				}

				return null;
			}
			catch(NullReferenceException)
			{
				return null;
			}
		}
		private Double? GetTotalSupply(HtmlDocument htmlDoc)
		{
			try
			{
				var maxSupplyOrTotalSupplyHeader = htmlDoc.DocumentNode.SelectSingleNode(CoinMarketCapPaths.MaxSupplyOrTotalSupplyHeader).InnerText.Replace("\n", "");
				var maxSupplyOrTotalSupplyValueStr = htmlDoc.DocumentNode.SelectSingleNode(CoinMarketCapPaths.MaxSupplyOrTotalSupplyValue).InnerText.Replace("\n", "");
				if(Double.TryParse(maxSupplyOrTotalSupplyValueStr, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var maxSupplyOrTotalSupplyValue))
				{
					if(maxSupplyOrTotalSupplyHeader.Equals("Total Supply"))
					{
						return maxSupplyOrTotalSupplyValue;
					}
				}

				return null;
			}
			catch(NullReferenceException)
			{
				return null;
			}
		}


		/// <summary>
		/// Step 2: Save OHLC using xpath to tab content table with ohlcv + market cap data
		/// </summary>
		private HistoricalDataDto[] CollectHistoricalData(HtmlDocument htmlDoc)
		{
			try
			{
				var collectedDailyOhlcList = new List<HistoricalDataDto>();

				var historicalDataNodes = htmlDoc.DocumentNode.SelectNodes(CoinMarketCapPaths.HistoricalDataTable);
				foreach(var x in historicalDataNodes)
				{
					collectedDailyOhlcList.Add(CreateHistoricalDataItem(x));
				}

				return collectedDailyOhlcList.ToArray();
			}
			catch(NullReferenceException ex)
			{
				return new HistoricalDataDto[] { };
			}
		}
		private HistoricalDataDto CreateHistoricalDataItem(HtmlNode node)
		{
			var historicalData = new HistoricalDataDto();
			var values = node.InnerText.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

			if(DateTime.TryParse(values[0], out var timeOpen))
			{
				historicalData.TimeOpen = timeOpen.ToPosixTimeMs();
			}
			if(Double.TryParse(values[1], out var open))
			{
				historicalData.Open = open;
			}
			if(Double.TryParse(values[2], out var high))
			{
				historicalData.High = high;
			}
			if(Double.TryParse(values[3], out var low))
			{
				historicalData.Low = low;
			}
			if(Double.TryParse(values[4], out var close))
			{
				historicalData.Close = close;
			}
			if(Double.TryParse(values[5], out var volume))
			{
				historicalData.Volume = volume;
			}
			else
			{
				historicalData.Volume = 0;
			}
			if(Double.TryParse(values[6], out var marketCap))
			{
				historicalData.MarketCap = marketCap;
			}
			else
			{
				historicalData.MarketCap = 0;
			}

			return historicalData;
		}


		private Boolean TryGetPage(String url, out HtmlDocument page)
		{
			var attempts = 10;
			const Int32 attemptDelay = 3000;
			var htmlWeb = new HtmlWeb();

			do
			{
				try
				{
					page = htmlWeb.Load(url);
					return true;
				}
				catch(Exception)
				{
					Console.WriteLine($"Page load failed: {url} | Attempts left: {attempts}");
					attempts--;
					Thread.Sleep(attemptDelay);
				}
			} while(attempts > 0);

			page = new HtmlDocument();
			return false;
		}
	}
}