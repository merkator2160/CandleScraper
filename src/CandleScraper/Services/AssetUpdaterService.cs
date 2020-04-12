using AutoMapper;
using CandleScraper.Database.Interfaces;
using CandleScraper.Database.Models.Storage;
using CandleScraper.ExternalApi.CoinMarketCapPro.Enums;
using CandleScraper.ExternalApi.CoinMarketCapPro.Interfaces;
using CandleScraper.ExternalApi.CoinMarketCapPro.Models;
using CandleScraper.Services.Interfaces;
using CandleScraper.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CandleScraper.Services
{
	internal class AssetUpdaterService : IAssetUpdaterService
	{
		private readonly ICoinMarketCapProClient _coinMarketCapProClient;
		private readonly IRepositoryBundle _repositoryBundle;
		private readonly IMapper _mapper;


		public AssetUpdaterService(
			ICoinMarketCapProClient coinMarketCapProClient,
			IRepositoryBundle repositoryBundle,
			IMapper mapper)
		{
			_coinMarketCapProClient = coinMarketCapProClient;
			_repositoryBundle = repositoryBundle;
			_mapper = mapper;
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
		public async Task<AssetDto[]> CollectCryptoCurrencyAssetInfoAsync(Int64[] cryptoMapIds)
		{
			var assetList = new List<AssetDb>();

			var cryptoInfos = await _coinMarketCapProClient.GetCryptoCurrencyInfoAsync(cryptoMapIds);
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

			return _mapper.Map<AssetDto[]>(assetList);
		}
		public async Task AddOrUpdateDatabaseAssetsAsync(AssetDto[] assets)
		{
			foreach(var x in assets)
			{
				var assetDb = await _repositoryBundle.Assets.GetByCoinMarketCapAssetIdAsync(x.CoinMarketCapAssetId);
				if(assetDb == null)
				{
					await _repositoryBundle.Assets.AddAsync(_mapper.Map<AssetDb>(x));
				}
			}
		}


		// SUPPORT FUNCTIONS //////////////////////////////////////////////////////////////////////
		private async Task<CmcpCryptocurrencyMapDataApi[]> CollectCoinDataAsync(GetCryptocurrencyMapRequestApi request)
		{
			var cryptoMapsData = new List<CmcpCryptocurrencyMapDataApi>();

			while(true)
			{
				var lastExtracted = await _coinMarketCapProClient.GetCryptoCurrencyMapAsync(request);
				if(lastExtracted.Data == null || lastExtracted.Data.Length == 0)
					break;

				cryptoMapsData.AddRange(lastExtracted.Data);
				request.StartPage += request.PageSize;
			}

			return cryptoMapsData.ToArray();
		}
	}
}