using CandleScraper.Database.Interfaces;

namespace CandleScraper.Database
{
	public class RepositoryBundle : IRepositoryBundle
	{
		public RepositoryBundle(
			IAssetRepository assetRepository,
			IOhlcRepository ohlcRepository,
			IDatabaseManagementRepository databaseManagementRepository)
		{
			Assets = assetRepository;
			Ohlcs = ohlcRepository;
			Management = databaseManagementRepository;
		}


		// IUnitOfWork ////////////////////////////////////////////////////////////////////////////
		public IAssetRepository Assets { get; }
		public IOhlcRepository Ohlcs { get; }
		public IDatabaseManagementRepository Management { get; }
	}
}