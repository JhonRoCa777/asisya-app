using FluentValidation;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Middleware
{
    public class ValidatorMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint is null)
            {
                await _next(context);
                return;
            }

            var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (actionDescriptor is null)
            {
                await _next(context);
                return;
            }

            var bodyParam = actionDescriptor.Parameters
                .FirstOrDefault(p => p.BindingInfo?.BindingSource == BindingSource.Body);

            if (bodyParam is null)
            {
                await _next(context);
                return;
            }

            var bodyType = bodyParam.ParameterType;
            context.Request.EnableBuffering();

            string bodyContent;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                bodyContent = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            if (string.IsNullOrWhiteSpace(bodyContent))
            {
                await _next(context);
                return;
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var instance = JsonSerializer.Deserialize(bodyContent, bodyType, jsonOptions);
            if (instance is null)
            {
                await _next(context);
                return;
            }

            // Cambiado: usar el RequestServices (scope actual del request)
            var validatorType = typeof(IValidator<>).MakeGenericType(bodyType);
            var validator = context.RequestServices.GetService(validatorType);

            if (validator is null)
            {
                await _next(context);
                return;
            }

            var validateAsyncMethod = validatorType.GetMethod("ValidateAsync", new[] { bodyType, typeof(CancellationToken) });
            var task = (Task)validateAsyncMethod!.Invoke(validator, new object[] { instance, CancellationToken.None })!;
            await task.ConfigureAwait(false);

            var resultProp = task.GetType().GetProperty("Result");
            var result = (FluentValidation.Results.ValidationResult)resultProp!.GetValue(task)!;

            if (!result.IsValid) throw new ValidationException(result.Errors);

            await _next(context);
        }
    }
}
