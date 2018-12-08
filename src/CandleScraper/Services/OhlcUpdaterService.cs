using AutoMapper;
using CandleScraper.Core.Consts;
using CandleScraper.Core.Helpers;
using CandleScraper.Database.Interfaces;
using CandleScraper.Database.Models.Filters;
using CandleScraper.Database.Models.Storage;
using CandleScraper.Services.Interfaces;
using CandleScraper.Services.Models;
using HtmlAgilityPack;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace CandleScraper.Services
{
	public class OhlcUpdaterService : IOhlcUpdaterService
	{
		private readonly IRepositoryBundle _repositoryBundle;
		private readonly IMapper _mapper;


		public OhlcUpdaterService(IRepositoryBundle repositoryBundle, IMapper mapper)
		{
			_repositoryBundle = repositoryBundle;
			_mapper = mapper;
		}


		// IOhlcUpdaterService ////////////////////////////////////////////////////////////////////
		public DailyOhlcDto[] ScrapeForAsset(String startDate, String endDate, AssetDb asset)
		{
			var newCryptoDailyOhlcDbList = new List<CollectedDailyOhlcDto>();

			if(!TryGetPage($"https://coinmarketcap.com/currencies/{asset.Slug}/historical-data/?start={startDate}&end={endDate}", out var htmlDoc))
				return new DailyOhlcDto[] { };

			var coinSummary = CollectCoinSummary(htmlDoc);
			var historicalData = CollectHistoricalData(htmlDoc);

			foreach(var x in historicalData)
			{
				if(x.IsProperlyCollected)
				{
					newCryptoDailyOhlcDbList.Add(new CollectedDailyOhlcDto()
					{
						AssetId = asset.Id,
						AssetName = asset.Name,
						CoinSummary = coinSummary,
						HistoricalData = x
					});
				}
			}

			return _mapper.Map<DailyOhlcDto[]>(newCryptoDailyOhlcDbList);
		}
		public async Task UpdateDatabaseDailyOhlcAsync(DailyOhlcDto[] ohlcs)
		{
			var ohlcDbToAdd = new List<DailyOhlcDb>();
			foreach(var x in ohlcs)
			{
				var existingOhlc = await _repositoryBundle.Ohlcs.GetCryptoDailyOhlcFilteredAsync(new GetCryptoDailyOhlcFilterDb()
				{
					AssetId = new ObjectId(x.AssetId),
					TimeOpenStart = x.TimeOpen,
					TimeOpenEnd = x.TimeOpen + 1.DaysToMs() - 1
				});
				if(existingOhlc != null && existingOhlc.Length == 0)
				{
					ohlcDbToAdd.Add(_mapper.Map<DailyOhlcDb>(x));

					Console.WriteLine($"Added new OHLC for Currency: {x.AssetName} and TimeOpen: {x.TimeOpen.FromPosixTimeMs():G}");
				}
			}

			if(ohlcDbToAdd.Count > 0)
				await _repositoryBundle.Ohlcs.AddRangeAsync(ohlcDbToAdd);
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