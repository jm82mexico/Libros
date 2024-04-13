using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tienda.Identity.Model;

namespace Tienda.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            
            builder.HasData(
                new ApplicationUser
                {
                    Id = "f803b71f-696e-4bc0-8c84-38b2811a775a",
                    Email = "juanchi@localhost.com",
                    NormalizedEmail = "JUANCHI@LOCALHOST.COM",
                    Nombre = "Juachi",
                    Apellidos = "Del Angel",
                    UserName = "juanchi",
                    NormalizedUserName = "JUANCHI",
                    PasswordHash = hasher.HashPassword(new ApplicationUser(), "admin"),
                    EmailConfirmed = true,
                },
                new ApplicationUser
                {
                    Id = "d2e250d3-60ff-45aa-ae4b-d24297af0714",
                    Email = "luche@localhost.com",
                    NormalizedEmail = "LUCHE@LOCALHOST.COM",
                    Nombre = "Luche",
                    Apellidos = "Sanchez",
                    UserName = "luche",
                    NormalizedUserName = "LUCHE",
                    PasswordHash = hasher.HashPassword(new ApplicationUser(), "user"),
                    EmailConfirmed = true,
                }
        );
        }
    }
}