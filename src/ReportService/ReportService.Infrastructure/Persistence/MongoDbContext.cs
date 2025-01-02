using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ReportService.Domain.Entities;


namespace ReportService.Infrastructure.Persistence
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDb:ConnectionString").Value;
            var databaseName = configuration.GetSection("MongoDb:DatabaseName").Value;

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }
        public IMongoCollection<Report> Reports => _database.GetCollection<Report>("Reports");
    }
}
