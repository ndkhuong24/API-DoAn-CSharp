using API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StyleController : ControllerBase
    {
        private readonly IStyleRepository _styleRepository;

        public StyleController(IStyleRepository styleRepository)
        {
            _styleRepository = styleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStyle()
        {
            try
            {
                return Ok(await _styleRepository.GetAllStylesAsync());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStyle(int id)
        {
            var style = await _styleRepository.GetStylesAsync(id);
            return Ok(style);
        }
    }
}
