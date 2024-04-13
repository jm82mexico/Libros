using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Tienda.Application.Exeptions;

namespace Tienda.API.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (System.Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            string errorMessages = ExceptionMessages(ex);

            ProblemDetails problemDetails = ex switch
            {
                DomainException _ => CreateProblemDetails(GetProblemType("6.5.1"), "One or more validation errors occurred.", (int)HttpStatusCode.BadRequest, errorMessages, context),

                ValidationExcepction _ => CreateProblemDetails(GetProblemType("6.5.1"), "One or more validation errors occurred.", (int)HttpStatusCode.BadRequest, errorMessages, context),

                InvalidOperationException when ex.InnerException is MySql.Data.MySqlClient.MySqlException => CreateProblemDetails(GetProblemType("6.6.1"), "Database Error", (int)HttpStatusCode.InternalServerError, errorMessages, context),

                InvalidOperationException _ => CreateProblemDetails(GetProblemType("6.6.1"), "Server Error", (int)HttpStatusCode.InternalServerError, $"An internal server has occurred - {errorMessages}", context),

                Microsoft.EntityFrameworkCore.DbUpdateException _ => CreateProblemDetails(GetProblemType("6.5.1"), "Database Error", (int)HttpStatusCode.BadRequest, errorMessages, context),

                ForbiddenException _ => CreateProblemDetails(GetProblemType("6.5.3"), "Authorization Exception", (int)HttpStatusCode.Forbidden, errorMessages, context),

                NotFoundException _ => CreateProblemDetails(GetProblemType("6.5.4"), "Not Found Exception", (int)HttpStatusCode.NotFound, errorMessages, context),

                _ => CreateProblemDetails(GetProblemType("6.6.1"), "Server Error", (int)HttpStatusCode.InternalServerError, $"An internal server has occurred - {errorMessages}", context),
            };

            _logger.LogError(ex, $"{problemDetails.Title}, {problemDetails.Detail}");
            await WriteResponseAsync(context, problemDetails);
        }

        private async Task WriteResponseAsync(HttpContext context, ProblemDetails problemDetails)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = problemDetails.Status!.Value;
            string result = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(result);
        }

        private ProblemDetails CreateProblemDetails(string type, string title, int status, string detail, HttpContext context)
        {
            return new ProblemDetails
            {
                Type = type,
                Title = title,
                Status = status,
                Detail = detail,
                Instance = context.Request.Path,
            };
        }

        private string GetProblemType(string section)
        {
            return $"https://tools.ietf.org/html/rfc7231#section-{section}";
        }

        private static string ExceptionMessages(Exception ex)
        {
            if (ex.InnerException == null)
            {
                return ex.Message;
            }

            return ex.Message + " -> " + ExceptionMessages(ex.InnerException);
        }
    }
}