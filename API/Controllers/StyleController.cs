using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StyleController : ControllerBase
    {
        private readonly StyleDbcontext _dbContext;

        public StyleController(StyleDbcontext styleDbcontext)
        {
            _dbContext = styleDbcontext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStyle()
        {
            var listStyle = await _dbContext.Style.FromSqlRaw("EXEC Style_Get").ToListAsync();
            return Ok(listStyle);
        }
    }
}
