using API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProductAsync()
        {
            try
            {
                return Ok(await _productRepository.GetAllProductAsync());
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            try
            {
                return Ok(await _productRepository.GetProductAsync(id));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
