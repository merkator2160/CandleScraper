using CandleScraper.Database.Interfaces;
using MongoDB.Driver;
using System;
using System.Linq;

namespace CandleScraper.Database.Repositories
{
	public class DatabaseManagementRepository : IDatabaseManagementRepository
	{
		private readonly IMongoManagement _mongoManagement;


		public DatabaseManagementRepository(IMongoManagement mongoManagement)
		{
			_mongoManagement = mongoManagement;
		}


		// IDatabaseManagementRepository //////////////////////////////////////////////////////////
		public String[] GetAllCollections()
		{
			using(var collections = _mongoManagement.Database.ListCollections())
			{
				var collectionList = collections.ToList();
				return collectionList.Select(x => x["name"].ToString()).ToArray();
			}
		}
	}
}