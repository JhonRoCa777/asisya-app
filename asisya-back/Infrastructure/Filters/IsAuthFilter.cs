using Application.Ports.Output;
using Domain.Entities;
using Domain.Errors;
using Domain.Externals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Filters
{
    public class IsAuthFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var _JwtRepository = context.HttpContext.RequestServices.GetRequiredService<IJwtRepository>();
            var _JwtSettingsDTO = context.HttpContext.RequestServices.GetRequiredService<JwtSettings>();

            var Token = context.HttpContext.Request.Cookies[_JwtSettingsDTO.Name];

            var Result = _JwtRepository.ValidateToken(Token!);

            var Error = new ObjectResult(Result<EmployeeDTO>.Failure(new ForbiddenError())){ StatusCode = 200 };

            if (!Result.IsSuccess)
            {
                context.Result = Error;
                return;
            }

            var SubClaim = Result.Data.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            if (SubClaim == null)
            {
                context.Result = Error;
                return;
            }

            context.HttpContext.Items[_JwtSettingsDTO.Name] = SubClaim.Value;
        }
    }
}
