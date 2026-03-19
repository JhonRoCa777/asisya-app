using Domain.Externals;
using System.Security.Claims;

namespace Application.Ports.Output
{
    public interface IJwtRepository
    {
        Result<string> GenerateToken(string UserId);

        Result<ClaimsPrincipal> ValidateToken(string Token);
    }
}
