using Autofac;
using CandleScraper.Database.Interfaces;
using CandleScraper.Services.Interfaces;
using CandleScraper.Services.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CandleScraper
{
	internal class ApplicationContext : IDisposable
	{
		private readonly ILifetimeScope _lifetimeScope;


		public ApplicationContext(ILifetimeScope lifetimeScope)
		{
			_lifetimeScope = lifetimeScope;
		}


		// FUNCTIONS ////////////////////////////////////////////////////////////////////////////////////
		public async Task RunAsync()
		{
			await CollectCryptoCurrencyAssetsAsync();
			await ScrapeOhlcsAsync();
		}
		private async Task CollectCryptoCurrencyAssetsAsync()
		{
			Console.WriteLine("Updating currency assets");
			using(var scope = _lifetimeScope.BeginLifetimeScope())
			{
				var assetUpdaterService = scope.Resolve<IAssetUpdaterService>();
				var cryptoMaps = await assetUpdaterService.CollectCryptoCurrencyMapAsync();

				const Int32 chunkSize = 300;
				var ids = cryptoMaps.Select(p => p.Id).ToArray();
				for(var i = 0; i < ids.Length; i = i + chunkSize)     // Must be chunked because request URL gets too long with all id values
				{
					var chunkedIds = ids.Skip(i).Take(chunkSize).ToArray();
					var assets = await assetUpdaterService.CollectCryptoCurrencyAssetInfoAsync(chunkedIds);
					await assetUpdaterService.AddOrUpdateDatabaseAssetsAsync(assets);

					Console.WriteLine($"Fetched Coin Market Cap assets {i + chunkedIds.Length} from {ids.Length}");
					Thread.Sleep(TimeSpan.FromSeconds(45));    // To prevent API 429 Too Many Requests because of starter plan
				}
			}
		}
		private async Task ScrapeOhlcsAsync()
		{
			Console.WriteLine("Updating OHLCs");
			var startDate = new DateTime(2013, 1, 1);
			var endDate = DateTime.UtcNow.AddDays(1);   // query for future 1 day. No data will be returned for tomorrow's date, but this is +1 day is a buffer

			ParallelOhlcScrapingRequestDto[] allCryptoAssetDb;
			using(var scope = _lifetimeScope.BeginLifetimeScope())
			{
				var assetCount = 0;
				var unitOfWork = scope.Resolve<IRepositoryBundle>();
				var totalAssetCount = await unitOfWork.Assets.GetQuantityAsync();

				allCryptoAssetDb = (await unitOfWork.Assets.GetAllAsync())
					.Select(p => new ParallelOhlcScrapingRequestDto()
					{
						Asset = p,
						StartDate = startDate.ToString("yyyyMMdd"),
						EndDate = endDate.ToString("yyyyMMdd"),
						SequenceNumber = assetCount++,
						TotalNumber = totalAssetCount
					})
					.ToArray();
			}

			allCryptoAssetDb
				.OrderBy(p => p.Asset.Id)
				.AsParallel()
				.AsOrdered()
				.WithExecutionMode(ParallelExecutionMode.Default)
				.WithDegreeOfParallelism(6)
				.ForAll(ScrapeOhlcParallelLinqThread);
		}


		// IDisposable ////////////////////////////////////////////////////////////////////////////
		public void Dispose()
		{
			_lifetimeScope?.Dispose();
		}


		// THREADS ////////////////////////////////////////////////////////////////////////////////
		private void ScrapeOhlcParallelLinqThread(ParallelOhlcScrapingRequestDto request)
		{
			using(var scope = _lifetimeScope.BeginLifetimeScope())
			{
				Console.WriteLine($"Currency: {request.Asset.Name} \t| Asset: {request.SequenceNumber} from {request.TotalNumber}");

				var ohlcUpdaterService = scope.Resolve<IOhlcUpdaterService>();
				var collectedDailyOhlcs = ohlcUpdaterService.ScrapeForAsset(request.StartDate, request.EndDate, request.Asset);
				ohlcUpdaterService.UpdateDatabaseDailyOhlcAsync(collectedDailyOhlcs).GetAwaiter().GetResult();
			}
		}
	}
}