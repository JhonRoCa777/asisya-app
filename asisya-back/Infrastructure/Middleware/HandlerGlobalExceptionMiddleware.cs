using Domain.Errors;
using Domain.Externals;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Infrastructure.Middleware
{
    public class HandlerGlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HandlerGlobalExceptionMiddleware> _logger;

        public HandlerGlobalExceptionMiddleware(RequestDelegate next, ILogger<HandlerGlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleAsync(context, ex);
            }
        }

        private async Task HandleAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            Object response;
            int statusCode;

            switch (ex)
            {
                case ValidationException ve:
                    statusCode = (int)HttpStatusCode.OK;
                    var dict = ve.Errors
                                 .GroupBy(e => e.PropertyName)
                                 .ToDictionary(
                                     g => g.Key,
                                     g => g.Select(e => e.ErrorMessage).ToArray()
                                 );
                    response = Result<object>.Failure(new UnprocessableEntityError(JsonSerializer.Serialize(dict)));
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(ex, "Error inesperado en la aplicación");
                    response = Result<object>.Failure(new InternalServerError());
                    break;
            }

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}