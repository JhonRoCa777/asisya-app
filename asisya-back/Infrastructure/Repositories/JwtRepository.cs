using Application.Ports.Output;
using Domain.Errors;
using Domain.Externals;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Repositories
{
    public class JwtRepository(JwtSettings Settings) : IJwtRepository
    {
        private readonly JwtSettings _Settings = Settings;

        public Result<string> GenerateToken(string UserId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_Settings.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, UserId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_Settings.ExpireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Result<string>.Success(tokenHandler.WriteToken(token));
        }

        public Result<ClaimsPrincipal> ValidateToken(string Token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler
                {
                    MapInboundClaims = false
                };

                var key = Encoding.UTF8.GetBytes(_Settings.Key);

                var principal = tokenHandler.ValidateToken(Token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return Result<ClaimsPrincipal>.Success(principal);
            }
            catch
            {
                return Result<ClaimsPrincipal>.Failure(new ForbiddenError());
            }
        }
    }
}
