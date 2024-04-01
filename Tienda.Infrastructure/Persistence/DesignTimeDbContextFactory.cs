using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace Tienda.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StreamerDbContext>
    {
        public StreamerDbContext CreateDbContext(string[] args)
        {
            try
                {
                    IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Tienda.API"))
                        .AddJsonFile("appsettings.json")
                        .Build();

                    var builder = new DbContextOptionsBuilder<StreamerDbContext>();
                    var connectionString = configuration.GetConnectionString("DefaultConnection");

                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        builder.UseMySQL(connectionString);
                    }

                    return new StreamerDbContext(builder.Options);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Excepción: {ex.Message}");
                    Console.WriteLine($"Excepción interna: {ex.InnerException?.Message}");
                    throw;
                }
        }
    }
}