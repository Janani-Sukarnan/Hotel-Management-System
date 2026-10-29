using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HotelManagementEFCore.Data
{
    public class HotelDbContextFactory
        : IDesignTimeDbContextFactory<HotelDbContext>
    {
        public HotelDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration =
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

            string connectionString =
                configuration.GetConnectionString("HotelDB")
                ?? throw new InvalidOperationException(
                    "HotelDB connection string was not found.");

            var options =
                new DbContextOptionsBuilder<HotelDbContext>()
                    .UseSqlServer(connectionString)
                    .Options;

            return new HotelDbContext(options);
        }
    }
}
