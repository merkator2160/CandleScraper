namespace CandleScraper.Database.Interfaces
{
	public interface IRepositoryBundle
	{
		IAssetRepository Assets { get; }
		IOhlcRepository Ohlcs { get; }
		IDatabaseManagementRepository Management { get; }
	}
}