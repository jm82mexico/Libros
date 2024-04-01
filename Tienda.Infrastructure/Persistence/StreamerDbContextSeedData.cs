using System.Text.Json;
using Microsoft.Extensions.Logging;
using Tienda.Domain;

namespace Tienda.Infrastructure.Persistence
{
    public class StreamerDbContextSeedData
    {
        public static async Task LoadDataAsync(StreamerDbContext DbContext, ILoggerFactory loggerFactory)
        {
            try
            {
                var videos = new List<Video>();
                if (!DbContext.Directores!.Any())
                {
                    var directorData = File.ReadAllText("../Tienda.Infrastructure/Data/director.json");                   
                    var directores = JsonSerializer.Deserialize<List<Director>>(directorData);
                    //validar si no es nulo
                    if (directores != null)
                    {
                        await DbContext.Directores!.AddRangeAsync(directores);
                        await DbContext.SaveChangesAsync();
                    }                    
                }

                if (!DbContext.Videos!.Any())
                {
                    var videoData = File.ReadAllText("../Tienda.Infrastructure/Data/video.json");
                    videos = JsonSerializer.Deserialize<List<Video>>(videoData);
                    //validar si no es nulo
                    if (videos != null)
                    {
                        await GetPreconfiguredVideoDirectorAsync(videos, DbContext);
                        await DbContext.SaveChangesAsync();                        
                    }
                }

                if (!DbContext.Actores!.Any())
                {
                    var actorData = File.ReadAllText("../Tienda.Infrastructure/Data/actor.json");
                    var actores = JsonSerializer.Deserialize<List<Actor>>(actorData);
                    //validar si no es nulo
                    if (actores != null && videos != null)
                    {
                        await DbContext.Actores!.AddRangeAsync(actores!);
                        await DbContext.AddRangeAsync(GetPreconfiguredVideoActor(videos));
                        await DbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<StreamerDbContextSeedData>();
                logger.LogError(ex.Message);
            }
        }

        private static async Task GetPreconfiguredVideoDirectorAsync(List<Video> videos, StreamerDbContext context)
        {
            

            var random = new Random();
            foreach (var video in videos)
            {
                video.DirectorId = random.Next(1, 99);
            }

            await context.Videos!.AddRangeAsync(videos);
        }

        private static IEnumerable<VideoActor> GetPreconfiguredVideoActor(List<Video> videos)
        {
            var videoActors = new List<VideoActor>();
            var random = new Random();

            foreach (var video in videos)
            {
                var videoActor = new VideoActor
                {
                    VideoId = video.Id,
                    ActorId = random.Next(1, 99)
                };
                videoActors.Add(videoActor);
            }

            return videoActors;
        }
    }
}