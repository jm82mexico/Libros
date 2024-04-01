
using Microsoft.Extensions.Logging;
using Tienda.Domain;

namespace Tienda.Infrastructure.Persistence
{
    public class StreamerDbContextSeed
    {
        public static async Task SeedAsync(StreamerDbContext DbContext,ILoggerFactory loggerFactory)
        {
            if (!DbContext.Streamers!.Any())
            {
                var logger = loggerFactory.CreateLogger<StreamerDbContextSeed>();
                DbContext.Streamers!.AddRange(GetPreconfiguredStreamers());
                await DbContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(StreamerDbContext).Name);

            }
        }


        private static IEnumerable<Streamer> GetPreconfiguredStreamers()
        {
            return new List<Streamer>()
            {
                new Streamer { CreatedBy = "Juanchi",Nombre = "Juanchi HBO",Url = "https://www.youtube.com/watch?v=1M7q8xQf9qo"},
                new Streamer { CreatedBy = "Juanchi",Nombre = "Amazon VIP",Url = "https://www.youtube.com/watch?v=1M7q8xQf9qo"},
            };
        }
    }
}