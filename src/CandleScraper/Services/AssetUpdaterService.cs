using CandleScraper.Database.Interfaces;
using CandleScraper.Database.Models.Storage;
using CandleScraper.ExternalApi.CoinMarketCapPro.Enums;
using CandleScraper.ExternalApi.CoinMarketCapPro.Interfaces;
using CandleScraper.ExternalApi.CoinMarketCapPro.Models;
using CandleScraper.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CandleScraper.Services
{
	internal class AssetUpdaterService : IAssetUpdaterService
	{
		private readonly ICoinMarketCapProClient _coinMarketCapProClient;
		private readonly IRepositoryBundle _repositoryBundle;


		public AssetUpdaterService(
			ICoinMarketCapProClient coinMarketCapProClient,
			IRepositoryBundle repositoryBundle)
		{
			_coinMarketCapProClient = coinMarketCapProClient;
			_repositoryBundle = repositoryBundle;
		}


		// IAssetUpdaterService ////////////////////////////////////////////////////////////////////
		public async Task<AssetDb[]> GetAssetsFromMarketAsync()
		{
			var cryptoMapsData = await CollectCryptocurrencyMapAsync();
			var assets = await CollectCryptoCurrencyInfoAsync(cryptoMapsData);

			return assets;
		}
		public async Task UpdateDatabaseAssetsAsync(AssetDb[] assets)
		{
			var workingStep = 100;
			for(var i = 0; i < assets.Length; i = i + workingStep)      // Must chunck because EF execution time is long
			{
				var selectedAssets = assets.Skip(i).Take(workingStep).ToArray();
				foreach(var x in selectedAssets)
				{
					var assetDb = await _repositoryBundle.Assets.GetAsync(x.Id);
					if(assetDb == null)
					{
						await _repositoryBundle.Assets.AddAsync(x);
					}
					else
					{
						_repositoryBundle.Assets.Update(assetDb);
					}
				}
			}
		}


		// SUPPORT FUNCTIONS //////////////////////////////////////////////////////////////////////
		private async Task<CmcpCryptocurrencyMapDataApi[]> CollectCryptocurrencyMapAsync()
		{
			var cryptoMapsData = new List<CmcpCryptocurrencyMapDataApi>();

			var startPage = 1;
			var pageSize = 5000; // API max

			cryptoMapsData.AddRange(await CollectCoinDataAsync(new GetCryptocurrencyMapRequestApi()
			{
				ListingStatus = ListingStatusApi.Active,
				PageSize = pageSize,
				StartPage = startPage
			}));
			cryptoMapsData.AddRange(await CollectCoinDataAsync(new GetCryptocurrencyMapRequestApi()
			{
				ListingStatus = ListingStatusApi.Inactive,
				PageSize = pageSize,
				StartPage = startPage
			}));

			return cryptoMapsData.ToArray();
		}
		private async Task<CmcpCryptocurrencyMapDataApi[]> CollectCoinDataAsync(GetCryptocurrencyMapRequestApi request)
		{
			var cryptoMapsData = new List<CmcpCryptocurrencyMapDataApi>();

			while(true)
			{
				var lastExtracted = await _coinMarketCapProClient.GetCryptocurrencyMapAsync(request);
				if(lastExtracted.Data == null || lastExtracted.Data.Length == 0)
					break;

				cryptoMapsData.AddRange(lastExtracted.Data);
				request.StartPage += request.PageSize;
			}

			return cryptoMapsData.ToArray();
		}
		private async Task<AssetDb[]> CollectCryptoCurrencyInfoAsync(CmcpCryptocurrencyMapDataApi[] cryptoMapsData)
		{
			var assetList = new List<AssetDb>();
			var ids = cryptoMapsData.Select(p => p.Id).ToArray();

			for(var i = 0; i < ids.Length; i = i + 300)     // Must chunck because request URL gets too long with all values of ids
			{
				var chunkedIds = ids.Skip(i).Take(300).ToArray();
				var cryptoInfos = await _coinMarketCapProClient.GetCryptocurrencyInfoAsync(chunkedIds, null);
				foreach(var cmcId in chunkedIds)
				{
					var cryptoInfo = cryptoInfos.Data[cmcId.ToString()];
					var cryptoMap = cryptoMapsData.First(p => p.Id == cmcId);

					var asset = new AssetDb()
					{
						Id = cryptoInfo.Id,
						Currency = cryptoInfo.Symbol,
						Name = cryptoInfo.Name,
						Slug = cryptoInfo.Slug,
						LogoUrl = cryptoInfo.Logo,
						IsActivelyTraded = (cryptoMap.IsActive == 1),
						ProofType = String.Empty,
						TokenStandard = String.Empty,
						Category = (cryptoInfo.Category ?? String.Empty),
						IndustrySector = String.Empty,
						Algorithm = String.Empty
					};
					assetList.Add(asset);
				}
				Console.WriteLine($"{DateTime.UtcNow}: Fetched Coin Market Cap assets {i + 1} to {i + 1 + 300}");
				Thread.Sleep(TimeSpan.FromSeconds(45));    // To prevent API 429 Too Many Requests because of starter plan
			}

			return assetList.ToArray();
		}
	}
}