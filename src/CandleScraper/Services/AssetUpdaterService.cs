using CandleScraper.Database.Interfaces;
using CandleScraper.Database.Models.Storage;
using CandleScraper.ExternalApi.CoinMarketCapPro.Enums;
using CandleScraper.ExternalApi.CoinMarketCapPro.Interfaces;
using CandleScraper.ExternalApi.CoinMarketCapPro.Models;
using CandleScraper.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
		public async Task<CmcpCryptocurrencyMapDataApi[]> CollectCryptoCurrencyMapAsync()
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
		public async Task<AssetDb[]> CollectCryptoCurrencyAssetInfoAsync(Int64[] cryptoMapIds)
		{
			var assetList = new List<AssetDb>();

			var cryptoInfos = await _coinMarketCapProClient.GetCryptocurrencyInfoAsync(cryptoMapIds);
			foreach(var x in cryptoMapIds)
			{
				var cryptoInfo = cryptoInfos.Data[x.ToString()];
				var asset = new AssetDb()
				{
					CoinMarketCapAssetId = cryptoInfo.Id,
					Currency = cryptoInfo.Symbol,
					Name = cryptoInfo.Name,
					Slug = cryptoInfo.Slug,
					LogoUrl = cryptoInfo.Logo,
					Category = cryptoInfo.Category
				};
				assetList.Add(asset);
			}

			return assetList.ToArray();
		}
		public async Task AddOrUpdateDatabaseAssetsAsync(AssetDb[] assets)
		{
			var workingStep = 100;
			for(var i = 0; i < assets.Length; i = i + workingStep)      // Must chunck because EF execution time is long
			{
				var selectedAssets = assets.Skip(i).Take(workingStep).ToArray();
				foreach(var x in selectedAssets)
				{
					var assetDb = await _repositoryBundle.Assets.GetByCoinMarketCapAssetIdAsync(x.CoinMarketCapAssetId);
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
	}
}