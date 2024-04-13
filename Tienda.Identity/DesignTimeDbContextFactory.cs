using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Tienda.Identity
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            try
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),"..","Tienda.API"))
                    .AddJsonFile("appsettings.json")
                    .Build();
                
                var builder = new DbContextOptionsBuilder<IdentityDbContext>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Connection string not found");
                }

                builder.UseMySQL(connectionString);

                return new IdentityDbContext(builder.Options);
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