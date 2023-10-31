using API.Data;
using API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/Style")]
    [ApiController]
    public class StyleController : ControllerBase
    {
        private readonly IStyleRepository _styleRepository;

        public StyleController(IStyleRepository styleRepository)
        {
            _styleRepository = styleRepository;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllStyleActice()
        {
            try
            {
                return Ok(await _styleRepository.GetAllStylesActiveAsync());
            }
            catch
            {
                return BadRequest();
            }
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
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetStyleAcync(int id)
        {
            try
            {
                return Ok(await _styleRepository.GetStyleAcync(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetStyleByNameAcync(string name)
        {
            try
            {
                return Ok(await _styleRepository.GetStyleByNameAcync(name));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostStyle(Style style)
        {
            var styles = await _styleRepository.AddStyleAsync(style);
            return Ok(styles);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStyle(int id)
        {
            await _styleRepository.DeleteStyleAsync(id);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateStyle(Style style)
        {
            await _styleRepository.UpdateStyleAsync(style);
            return Ok();
        }
    }
}
