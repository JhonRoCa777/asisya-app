using Application.Ports.Input;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController(ICategoryService Service)
        : ControllerBase
    {
        private readonly ICategoryService _Service = Service;

        [HttpGet]
        public async Task<IActionResult> Index()
            => Ok(await _Service.FindAllAsync());
    }
}
