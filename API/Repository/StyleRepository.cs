using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class StyleRepository : IStyleRepository
    {
        private readonly StyleDbcontext _dbcontext;

        public StyleRepository(StyleDbcontext styleDbcontext) {
            _dbcontext = styleDbcontext;
        } 
        public async Task<int> AddStyleAsync(Style style)
        {
            var newStyle = style;
            _dbcontext.Style!.Add(newStyle);
            await _dbcontext.SaveChangesAsync();
            return newStyle.id;
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
            var styles = await _dbcontext.Style!.ToListAsync();
            return styles;

        }

        public async Task<Style> GetStylesAsync(int id)
        {
            var style = await _dbcontext.Style!.FindAsync(id);
            return style;
        }

        public async Task UpdateStyleAsync(int id, Style style)
        {
            if(id==style.id)
            {
                var updateStyle = style;
                _dbcontext.Style!.Update(updateStyle);
                await _dbcontext.SaveChangesAsync();
            }
        }
    }
}
