using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ReportService.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace ReportService.Infrastructure.Repository
{
    public class MongoRepository<T> : IRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(MongoDbContext dbContext, IConfiguration configuration)
        {
            var collectionName = configuration[$"MongoDb:CollectionNames:{typeof(T).Name}"];
            if (string.IsNullOrWhiteSpace(collectionName))
            {
                throw new ArgumentException($"Collection name for type {typeof(T).Name} is not configured.");
            }

            _collection = dbContext.GetCollection<T>(collectionName);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Guid id, T entity)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}
