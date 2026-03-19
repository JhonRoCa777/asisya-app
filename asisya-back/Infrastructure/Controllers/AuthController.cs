using Application.Ports.Output;
using Domain.Entities;
using Domain.Externals;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        IJwtRepository JwtRepository, JwtSettings Settings, IEmployeeRepository Repository
    ) : ControllerBase
    {
        private readonly IJwtRepository _JwtRepository = JwtRepository;
        private readonly JwtSettings _Settings = Settings;
        private readonly IEmployeeRepository _Repository = Repository;

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] EmployeeLoginDTO EmployeeLoginDTO)
        {
            var EmployeeDTO = await _Repository.FindByDocumentAsync(EmployeeLoginDTO.Document, EmployeeLoginDTO.DocumentType);

            if (EmployeeDTO.IsSuccess)
            {
                var Result = _JwtRepository.GenerateToken(EmployeeDTO.Data.EmployeeID.ToString());

                Response.Cookies.Append(_Settings.Name, Result.Data, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(_Settings.ExpireMinutes)
                });

                return Ok(Result<bool>.Success(true));
            }

            return Ok(EmployeeDTO);
        }

        [HttpGet("[action]")]
        public IActionResult Verify()
            => Ok(_JwtRepository.ValidateToken(Request.Cookies[_Settings.Name]!));

        [HttpGet("[action]")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(_Settings.Name);
            return Ok();
        }
    }
}