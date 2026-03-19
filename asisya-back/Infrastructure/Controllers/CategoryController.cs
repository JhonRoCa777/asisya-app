using Application.Ports.Input;
using Domain.Entities.Category;
using Domain.Externals;
using Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IsAuthFilter]
    public class CategoryController(ICategoryService Service, JwtSettings Settings)
        : ControllerBase
    {
        private readonly ICategoryService _Service = Service;
        private readonly JwtSettings _Settings = Settings;

        [HttpGet]
        public async Task<IActionResult> Index()
            => Ok(await _Service.FindAllAsync());

        [HttpGet("{EntityId}")]
        public async Task<IActionResult> Get(Guid EntityId)
            => Ok(await _Service.GetAsync(EntityId));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryRequestDTO EntityRequest)
            => Ok(await _Service.CreateAsync(
                    EntityRequest,
                    Guid.TryParse(HttpContext.Items[_Settings.Name]?.ToString(), out var userId) ? userId : Guid.Empty
             ));

        [HttpPut("{EntityId}")]
        public async Task<IActionResult> Put(Guid EntityId, [FromBody] CategoryRequestDTO EntityRequest)
            => Ok(await _Service.UpdateAsync(
                    EntityId,
                    EntityRequest,
                    Guid.TryParse(HttpContext.Items[_Settings.Name]?.ToString(), out var userId) ? userId : Guid.Empty
             ));

        [HttpDelete("{EntityId}")]
        public async Task<IActionResult> Delete(Guid EntityId)
            => Ok(await _Service.DeleteAsync(
                    EntityId,
                    Guid.TryParse(HttpContext.Items[_Settings.Name]?.ToString(), out var userId) ? userId : Guid.Empty
             ));
    }
}
