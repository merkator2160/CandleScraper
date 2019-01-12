using CandleScraper.Database.Interfaces;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CandleScraper.Database.Repositories
{
	public class DatabaseManagementRepository : IDatabaseManagementRepository
	{
		private readonly DataContext _context;


		public DatabaseManagementRepository(DataContext context)
		{
			_context = context;
		}


		// IDatabaseManagementRepository //////////////////////////////////////////////////////////
		public async Task<String[]> GetAllCollectionsAsync()
		{
			using(var cursor = await _context.Database.ListCollectionNamesAsync())
			{
				return cursor.ToList().ToArray();
			}
		}
		public Task DropDatabaseAsync(String name)
		{
			return _context.Client.DropDatabaseAsync(name);
		}
		public Task DropCollectionAsync(String name)
		{
			return _context.Database.DropCollectionAsync(name);
		}
	}
}