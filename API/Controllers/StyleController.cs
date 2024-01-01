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
        public async Task<IActionResult> GetAllActiveStyles()
        {
            try
            {
                var styles = await _styleRepository.GetAllStylesActiveAsync();
                return Ok(styles);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest($"Failed to retrieve active styles: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStyles()
        {
            try
            {
                var styles = await _styleRepository.GetAllStylesAsync();
                return Ok(styles);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest($"Failed to retrieve styles: {ex.Message}");
            }
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetStyleById(int id)
        {
            try
            {
                var style = await _styleRepository.GetStyleAcync(id);
                return Ok(style);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest($"Failed to retrieve style by ID: {ex.Message}");
            }
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetStyleByName(string name)
        {
            try
            {
                var style = await _styleRepository.GetStyleByNameAcync(name);
                return Ok(style);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest($"Failed to retrieve style by name: {ex.Message}");
            }
        }

        [HttpGet("searchName/{name}")]
        public async Task<IActionResult> SearchByName(string name)
        {
            try
            {
                var styles = await _styleRepository.GetSearchNameAsync(name);
                return Ok(styles);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest($"Failed to perform style search by name: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostStyle(Style style)
        {
            try
            {
                var styles = await _styleRepository.AddStyleAsync(style);
                return Ok(styles);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest($"Failed to add style: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStyle(int id)
        {
            try
            {
                await _styleRepository.DeleteStyleAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest($"Failed to delete style: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStyle(Style style)
        {
            try
            {
                await _styleRepository.UpdateStyleAsync(style);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest($"Failed to update style: {ex.Message}");
            }
        }
    }
}


//using API.Data;
//using API.Repository;
//using Microsoft.AspNetCore.Mvc;

//namespace API.Controllers
//{
//    [Route("api/Style")]
//    [ApiController]
//    public class StyleController : ControllerBase
//    {
//        private readonly IStyleRepository _styleRepository;

//        public StyleController(IStyleRepository styleRepository)
//        {
//            _styleRepository = styleRepository;
//        }

//        [HttpGet("active")]
//        public async Task<IActionResult> GetAllStyleActice()
//        {
//            try
//            {
//                return Ok(await _styleRepository.GetAllStylesActiveAsync());
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }
//        [HttpGet]
//        public async Task<IActionResult> GetAllStyle()
//        {
//            try
//            {
//                return Ok(await _styleRepository.GetAllStylesAsync());
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }
//        [HttpGet("id/{id}")]
//        public async Task<IActionResult> GetStyleAcync(int id)
//        {
//            try
//            {
//                return Ok(await _styleRepository.GetStyleAcync(id));
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }
//        [HttpGet("name/{name}")]
//        public async Task<IActionResult> GetStyleByNameAcync(string name)
//        {
//            try
//            {
//                return Ok(await _styleRepository.GetStyleByNameAcync(name));
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }
//        [HttpGet("searchName/{name}")]
//        public async Task<IActionResult> GetSearchNameAsync(string name)
//        {
//            try
//            {
//                return Ok(await _styleRepository.GetSearchNameAsync(name));
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }
//        [HttpPost]
//        public async Task<IActionResult> PostStyle(Style style)
//        {
//            var styles = await _styleRepository.AddStyleAsync(style);
//            return Ok(styles);
//        }
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteStyle(int id)
//        {
//            await _styleRepository.DeleteStyleAsync(id);
//            return Ok();
//        }
//        [HttpPut]
//        public async Task<IActionResult> UpdateStyle(Style style)
//        {
//            await _styleRepository.UpdateStyleAsync(style);
//            return Ok();
//        }
//    }
//}
