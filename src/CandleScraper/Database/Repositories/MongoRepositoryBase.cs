using CandleScraper.Database.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandleScraper.Database.Repositories
{
	public class MongoRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IStorageEntity
	{
		private readonly IMongoCollection<TEntity> _collection;


		public MongoRepositoryBase(IMongoCollection<TEntity> collection)
		{
			_collection = collection;
		}


		// IRepository ////////////////////////////////////////////////////////////////////////////
		public TEntity Get(String id)
		{
			return _collection.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefault();
		}
		public async Task<TEntity> GetAsync(String id)
		{
			return await (await _collection.FindAsync(new BsonDocument("_id", new ObjectId(id)))).FirstOrDefaultAsync();
		}
		public TEntity[] GetAll()
		{
			return _collection.Find(new BsonDocument()).ToList().ToArray();
		}
		public async Task<TEntity[]> GetAllAsync()
		{
			return (await (await _collection.FindAsync(new BsonDocument())).ToListAsync()).ToArray();
		}
		public TEntity[] GetChunked(Int32 offset, Int32 amount)
		{
			return _collection.Find(new BsonDocument()).Skip(offset).Limit(amount).ToList().ToArray();
		}
		public async Task<TEntity[]> GetChunkedAsync(Int32 offset, Int32 amount)
		{
			return (await _collection.AsQueryable().Skip(offset).Take(amount).ToListAsync()).ToArray();
		}
		public Int64 GetQuantity()
		{
			return _collection.CountDocuments(new BsonDocument());
		}
		public async Task<Int64> GetQuantityAsync()
		{
			return await _collection.CountDocumentsAsync(new BsonDocument());
		}
		public void Add(TEntity item)
		{
			_collection.InsertOne(item);
		}
		public async Task AddAsync(TEntity item)
		{
			await _collection.InsertOneAsync(item);
		}
		public void AddRange(IEnumerable<TEntity> items)
		{
			_collection.InsertMany(items);
		}
		public async Task AddRangeAsync(IEnumerable<TEntity> items)
		{
			await _collection.InsertManyAsync(items);
		}
		public void Update(TEntity item)
		{
			_collection.ReplaceOne(new BsonDocument("_id", item.Id), item);
		}
		public void Delete(String id)
		{
			_collection.DeleteOne(new BsonDocument("_id", new ObjectId(id)));
		}
		public async Task DeleteAsync(String id)
		{
			await _collection.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
		}
		public void Remove(TEntity item)
		{
			_collection.DeleteOne(new BsonDocument("_id", item.Id));
		}
		public async Task RemoveAsync(TEntity item)
		{
			await _collection.DeleteOneAsync(new BsonDocument("_id", item.Id));
		}
		public void RemoveRange(IEnumerable<TEntity> items)
		{
			var ids = items.Select(p => p.Id).ToArray();
			_collection.DeleteMany(p => ids.Contains(p.Id));
		}
		public async Task RemoveRangeAsync(IEnumerable<TEntity> items)
		{
			var ids = items.Select(p => p.Id).ToArray();
			await _collection.DeleteOneAsync(p => ids.Contains(p.Id));
		}
	}
}