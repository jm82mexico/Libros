
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tienda.Application.Contracts.Infrastructure;
using Tienda.Application.Contracts.Persistence;
using Tienda.Application.Models;
using Tienda.Infrastructure.Email;
using Tienda.Infrastructure.Persistence;
using Tienda.Infrastructure.Repositories;

namespace Tienda.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<StreamerDbContext>(options =>
                options.UseMySQL(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IAsyncRepository<>),typeof(RepositoryBase<>));
            services.AddScoped<IStreamerRepository, StreamerRepository>();
            services.AddScoped<IVideoRepository, VideoRepository>();

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();


            return services;
        }
    }
}