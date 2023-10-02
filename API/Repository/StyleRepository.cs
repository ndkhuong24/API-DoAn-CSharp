using API.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace API.Repository
{
    public class StyleRepository : IStyleRepository
    {
        private readonly StyleDbcontext _dbcontext;

        public StyleRepository(StyleDbcontext styleDbcontext)
        {
            _dbcontext = styleDbcontext;
        }
        //public async Task<Style> AddStyleAsync(Style style)
        //{

        //    var styles = (await _dbcontext.Style!.FromSqlRaw("EXECUTE PostStyle @Id=0, @Name={0},@Status={1}", style.id, style.name, style.status).ToListAsync()).FirstOrDefault();
        //    var a = styles;
        //    return a;
        //}
        public async Task<Style> AddStyleAsync(Style style)
        {
            try
            {
                var nameParam = new SqlParameter("@Name", style.name);
                var statusParam = new SqlParameter("@Status", style.status);

                await _dbcontext.Database.ExecuteSqlRawAsync("EXEC PostStyle @Name, @Status", nameParam, statusParam);

                return style;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteStyleAsync(int id)
        {
            try
            {
                var nameParam = new SqlParameter("@Id", id);
                await _dbcontext.Database.ExecuteSqlRawAsync("EXEC DeleteStyle {0}", id);
            }
            catch (Exception ex)
            {
                throw ex;
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
            try
            {
                var ids = new SqlParameter("@Id", id);
                var name = new SqlParameter("@NewName", style.name);
                var status = new SqlParameter("@NewStatus", style.status);
                await _dbcontext.Database.ExecuteSqlRawAsync("EXEC UpdateStyle @Id,@NewName,@NewStatus", ids, name, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
