using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tienda.API.Middlewares
{
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description));
            }
        }

        public void Configure(string? name, SwaggerGenOptions options) => Configure(options);

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription desc)
        {
            var info = new OpenApiInfo()
            {
                Title = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name} - Powered by Nexonify®️",
                Version = desc.ApiVersion.ToString()
            };

            if (desc.IsDeprecated)
            {
                info.Description += " This API version has been deprecated. Please use one of the new APIs available from the explorer.";
            }

            return info;
        }
    }

    public class JwtAuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            bool RequiresAuthorization(Type type)
            {
                if (type == null)
                    return false;

                var allowsAnonymous = type.GetCustomAttributes<AllowAnonymousAttribute>().Any();
                if (allowsAnonymous)
                    return false;

                return type.GetCustomAttributes<AuthorizeAttribute>().Any() || RequiresAuthorization(type.BaseType!);
            }

            var allowsAnonymous = context.MethodInfo.GetCustomAttributes<AllowAnonymousAttribute>().Any();
            var requiresAuthorization = !allowsAnonymous && context.MethodInfo.CustomAttributes.OfType<AuthorizeAttribute>().Any();

            if (!allowsAnonymous && !requiresAuthorization)
                requiresAuthorization = RequiresAuthorization(context.MethodInfo.DeclaringType!);

            if (requiresAuthorization)
            {
                EnsureUnauthorizedResponse(operation);

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }] = new List<string>()
                    }
                };
            }
        }

        private static void EnsureUnauthorizedResponse(OpenApiOperation operation)
        {
            if (!operation.Responses.TryGetValue("401", out _))
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized - Requiere autenticación JWT" });
            }
        }
    }

    
}