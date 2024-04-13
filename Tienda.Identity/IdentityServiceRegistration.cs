using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Tienda.Application.Contracts.Identity;
using Tienda.Application.Models.Identity;
using Tienda.Identity.Model;
using Tienda.Identity.Services;

namespace Tienda.Identity
{
    public static class IdentityServiceRegistration
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddDbContext<IdentityDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                if (connectionString != null)
                {
                    options.UseMySQL(connectionString, b => b.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName));
                }
            });
            
            services.AddIdentity<ApplicationUser,IdentityRole>()
                                .AddEntityFrameworkStores<IdentityDbContext>()
                                .AddDefaultTokenProviders();
            services.AddTransient<IAuthService, AuthService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme ;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme ;
            }).AddJwtBearer(options =>
            {
                var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

                if (jwtSettings?.Issuer == null)
                {
                    throw new InvalidOperationException("El emisor del token JWT no puede ser nulo.");
                }

                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });

            return services;
        }
    }
}