using API.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace API.Repository
{
    public class StyleRepository : IStyleRepository
    {
        private readonly StyleDbcontext _dbcontext;

        public StyleRepository(StyleDbcontext styleDbcontext)
        {
            _dbcontext = styleDbcontext;
        }
        public async Task<Style> AddStyleAsync(Style style)
        {
            var styles = (await _dbcontext.Style!.FromSqlRaw("EXECUTE PostStyle @Name={0},@Status={1}", style.name, style.status).ToListAsync()).FirstOrDefault();
            return styles;
        }

        public async Task DeleteStyleAsync(int id)
        {
            var deleteStyle = _dbcontext.Style!.SingleOrDefault(b => b.id == id);
            if (deleteStyle != null)
            {
                _dbcontext.Style!.Remove(deleteStyle);
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task<List<Style>> GetAllStylesAsync()
        {
            var styles = await _dbcontext.Style!.FromSqlRaw("EXEC Style_Get").ToListAsync();
            return styles;

        }

        public async Task<Style> GetStylesAsync(int id)
        {
            var style = (await _dbcontext.Style!.FromSqlRaw("EXECUTE Style_Get {0}", id).ToListAsync()).FirstOrDefault();
            return style;
        }

        public async Task UpdateStyleAsync(int id, Style style)
        {
            if (id == style.id)
            {
                var updateStyle = style;
                _dbcontext.Style!.Update(updateStyle);
                await _dbcontext.SaveChangesAsync();
            }
        }
    }
}
