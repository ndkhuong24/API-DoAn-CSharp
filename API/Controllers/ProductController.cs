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
        [HttpGet("searchName/{name}")]
        public async Task<IActionResult> GetSearchNameAsync(string name)
        {
            try
            {
                return Ok(await _productRepository.GetSearchNameAsync(name));
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
        [HttpPut]
        public async Task<IActionResult> UpdateProductAsync(Product product)
        {
            try
            {
                await _productRepository.UpdateProductAsync(product);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
