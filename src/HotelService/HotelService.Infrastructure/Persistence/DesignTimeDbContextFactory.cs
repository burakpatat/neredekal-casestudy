
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HotelService.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HotelDbContext>
    {
        public HotelDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("env.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
            var connectionString = configuration.GetConnectionString("PostgreSqlConnection");

            optionsBuilder.UseNpgsql(connectionString);

            return new HotelDbContext(optionsBuilder.Options);
        }
    }
}
