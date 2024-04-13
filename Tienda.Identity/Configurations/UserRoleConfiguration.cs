using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tienda.Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "c9f08f2a-b10e-4cdc-819a-ff6fb43ac73f",
                    UserId = "f803b71f-696e-4bc0-8c84-38b2811a775a"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "29be0ad8-bedc-4da0-9d25-7cc989f39172",
                    UserId = "d2e250d3-60ff-45aa-ae4b-d24297af0714"
                }
                );
        }
    }
}