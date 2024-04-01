
using Microsoft.EntityFrameworkCore;
using Tienda.Application.Contracts.Persistence;
using Tienda.Domain;
using Tienda.Infrastructure.Persistence;

namespace Tienda.Infrastructure.Repositories
{
    public class VideoRepository : RepositoryBase<Video>, IVideoRepository
    {
        public VideoRepository(StreamerDbContext context) : base(context)
        {
        }

        public async Task<Video> GetVideoByNombre(string nombreVideo)
        {
            return await _context.Videos!.Where(v => v.Nombre == nombreVideo).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Video>> GetVideoByUsername(string username)
        {
            return await _context.Videos!.Where(v => v.CreatedBy == username).ToListAsync();
        }
    }
}