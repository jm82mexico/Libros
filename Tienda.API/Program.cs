using Tienda.Application;
using Tienda.Infrastructure;
using Tienda.Identity;


using Serilog;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Tienda.API.Middlewares;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Tienda.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    // Inicialización de Serilog 
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

    #region Common API Services
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    builder.Services.AddApiVersioning(opt =>
    {
        opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.ReportApiVersions = true;
        opt.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("x-api-version"),
            new MediaTypeApiVersionReader("x-api-version"));
    });
    builder.Services.AddVersionedApiExplorer(setup =>
    {
        setup.GroupNameFormat = "'v'VVV";
        setup.SubstituteApiVersionInUrl = true;
    });



    builder.Host.UseSerilog();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath, true);

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Autenticación JWT utilizando el esquema Bearer. Ejemplo: \"Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT"
        });
        c.OperationFilter<JwtAuthorizationOperationFilter>();
    });

    builder.Services.AddHttpContextAccessor();
    builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
    builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
    builder.Services.AddHttpClient();
    #endregion

    #region Services Registration
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddIdentityServices(builder.Configuration);
    #endregion

    builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
            });
    });

    var app = builder.Build();
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });
    }
    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }
    else
    {
        app.UseDeveloperExceptionPage();
    }
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseAuthentication();
    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    app.UseCors("CorsPolicy");
    app.MapControllers();

    //Configurar las migraciones
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        try
        {
            var context = services.GetRequiredService<StreamerDbContext>();
            await context.Database.MigrateAsync();
            await StreamerDbContextSeed.SeedAsync(context, loggerFactory);
            await StreamerDbContextSeedData.LoadDataAsync(context,loggerFactory);

            var contextIdentity = services.GetRequiredService<IdentityDbContext>();
            await contextIdentity.Database.MigrateAsync();
            
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while migrating the database.");
        }
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}