using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tienda.Identity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "c9f08f2a-b10e-4cdc-819a-ff6fb43ac73f",
                    Name = "Admin", 
                    NormalizedName = "ADMIN" 
                },
                new IdentityRole
                {
                    Id = "29be0ad8-bedc-4da0-9d25-7cc989f39172",
                    Name = "User", 
                    NormalizedName = "USER" 
                }
                );
        }
    }
}