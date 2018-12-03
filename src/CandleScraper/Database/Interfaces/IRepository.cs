using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CandleScraper.Database.Interfaces
{
	public interface IRepository<TEntity> where TEntity : class
	{
		TEntity Get(String id);
		Task<TEntity> GetAsync(String id);
		TEntity[] GetAll();
		Task<TEntity[]> GetAllAsync();
		TEntity[] GetChunked(Int32 offset, Int32 amount);
		Task<TEntity[]> GetChunkedAsync(Int32 offset, Int32 amount);
		Int64 GetQuantity();
		Task<Int64> GetQuantityAsync();
		void Add(TEntity item);
		Task AddAsync(TEntity item);
		void AddRange(IEnumerable<TEntity> items);
		Task AddRangeAsync(IEnumerable<TEntity> items);
		void Update(TEntity item);
		void Delete(String id);
		Task DeleteAsync(String id);
		void Remove(TEntity item);
		Task RemoveAsync(TEntity item);
		void RemoveRange(IEnumerable<TEntity> items);
		Task RemoveRangeAsync(IEnumerable<TEntity> items);
	}
}