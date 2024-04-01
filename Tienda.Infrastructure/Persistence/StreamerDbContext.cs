using Microsoft.EntityFrameworkCore;
using Tienda.Domain;
using Tienda.Domain.Common;

namespace Tienda.Infrastructure.Persistence
{
    public class StreamerDbContext : DbContext
    {
        public StreamerDbContext(DbContextOptions<StreamerDbContext> options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = "System";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = "System";
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Streamer>().ToTable("Streamer")
                .HasMany(m => m.Videos)
                .WithOne(m => m.Streamer)
                .HasForeignKey(m => m.StreamerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            //configurar las relaciones de la entidad Director
            modelBuilder.Entity<Director>()
                .HasMany(v => v.Videos)
                .WithOne(d => d.Director)
                .HasForeignKey(d => d.DirectorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            //Configuracion Anterior
            //modelBuilder.Entity<Video>().ToTable("Video")
            //    .HasMany(m => m.Actores)
            //    .WithMany(m => m.Videos)
            //    .UsingEntity<VideoActor>(
            //        pt => pt.HasKey( e => new {e.ActorId, e.VideoId})
            //    );

            //configurar las relaciones de la entidad Video         

            modelBuilder.Entity<Video>()
                .HasMany(a => a.Actores)
                .WithMany(v => v.Videos)
                .UsingEntity<VideoActor>(
                     j => j
                       .HasOne(p => p.Actor)
                       .WithMany(p => p.VideoActors)
                       .HasForeignKey(p => p.ActorId),
                    j => j
                        .HasOne(p => p.Video)
                        .WithMany(p => p.VideoActors)
                        .HasForeignKey(p => p.VideoId),
                    j =>
                    {
                        j.HasKey(t => new { t.ActorId, t.VideoId });
                    }
                );
            //ingnorar la creacion del id de la clase base
            modelBuilder.Entity<VideoActor>().Ignore(va => va.Id);
        }
        public DbSet<Streamer>? Streamers { get; set; }
        public DbSet<Video>? Videos { get; set; }
        public DbSet<Actor>? Actores { get; set; }
        public DbSet<Director>? Directores { get; set; }
    }
}