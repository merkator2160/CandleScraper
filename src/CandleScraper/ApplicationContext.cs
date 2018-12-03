using Autofac;
using CandleScraper.Database.Interfaces;
using CandleScraper.Services.Interfaces;
using CandleScraper.Services.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CandleScraper
{
	internal class ApplicationContext
	{
		private readonly IContainer _container;


		public ApplicationContext(IContainer container)
		{
			_container = container;
		}


		// FUNCTIONS ////////////////////////////////////////////////////////////////////////////////////
		public async Task RunAsync()
		{
			Console.WriteLine("Updating currency assets");
			using(var scope = _container.BeginLifetimeScope())
			{
				var assetUpdaterService = scope.Resolve<IAssetUpdaterService>();
				var assets = await assetUpdaterService.GetAssetsFromMarketAsync();
				await assetUpdaterService.UpdateDatabaseAssetsAsync(assets);
			}

			//Console.WriteLine("Updating OHLCs");
			////var startDate = DateTime.UtcNow.AddDays(-2);
			//var startDate = new DateTime(2013, 1, 1);
			//var endDate = DateTime.UtcNow.AddDays(1);   // query for future 1 day. No data will be returned for tomorrow's date, but this is +1 day is a buffer

			//await ScrapeOhlcParallelLinqAsync(startDate, endDate);

#if DEBUG
			Console.WriteLine("Done");
			Console.ReadKey();
#endif
		}

		private async Task ScrapeOhlcParallelLinqAsync(DateTime startDate, DateTime endDate)
		{
			ParallelOhlcScrapingRequestDto[] allCryptoAssetDb;
			using(var scope = _container.BeginLifetimeScope())
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


		// THREADS ////////////////////////////////////////////////////////////////////////////////
		private void ScrapeOhlcParallelLinqThread(ParallelOhlcScrapingRequestDto request)
		{
			using(var scope = _container.BeginLifetimeScope())
			{
				Console.WriteLine($"{DateTime.Now} | Currency: {request.Asset.Name} \t| Asset: {request.SequenceNumber} from {request.TotalNumber}");

				var ohlcUpdaterService = scope.Resolve<IOhlcUpdaterService>();
				var collectedDailyOhlcs = ohlcUpdaterService.ScrapeForAssetAsync(request.StartDate, request.EndDate, request.Asset)
					.GetAwaiter()
					.GetResult();
				ohlcUpdaterService.UpdateDatabaseDailyOhlcAsync(collectedDailyOhlcs)
					.GetAwaiter()
					.GetResult();
			}
		}
	}
}