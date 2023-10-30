using API.Data;
using API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/Product")]
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
        [HttpPost]
        public async Task<IActionResult> AddProductAsync(Product product)
        {
            try
            {
                var pd = await _productRepository.AddProductAsync(product);
                return Ok(pd);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            try
            {
                await _productRepository.DeleteProductAsync(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, Product product)
        {
            try
            {
                await _productRepository.UpdateProductAsync(id, product);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
