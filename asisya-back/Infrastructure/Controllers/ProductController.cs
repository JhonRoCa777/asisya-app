using Application.Ports.Input;
using Domain.Externals;
using Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IsAuthFilter]
    public class ProductController(IProductService Service, JwtSettings Settings)
        : ControllerBase
    {
        private readonly IProductService _Service = Service;
        private readonly JwtSettings _Settings = Settings;

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] string? nameFilter = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string orderBy = "CreatedAt",
            [FromQuery] bool orderAsc = false)
            => Ok(await _Service.FindAllAsync(nameFilter, pageNumber, pageSize, orderBy, orderAsc));

        [HttpPost("")]
        public async Task<IActionResult> Post(
            [FromQuery] int EntityRows,
            [FromQuery] int SimulateErrors = 0)
            => Ok(await _Service.CreateByRowsAsync(
                    EntityRows,
                    Guid.TryParse(HttpContext.Items[_Settings.Name]?.ToString(), out var userId) ? userId : Guid.Empty,
                    SimulateErrors
                ));

        [HttpDelete("{EntityId}")]
        public async Task<IActionResult> Delete(Guid EntityId)
            => Ok(await _Service.DeleteAsync(
                    EntityId,
                    Guid.TryParse(HttpContext.Items[_Settings.Name]?.ToString(), out var userId) ? userId : Guid.Empty
             ));
    }
}
