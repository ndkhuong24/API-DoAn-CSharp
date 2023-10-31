using API.Data;

namespace API.Repository
{
    public interface IStyleRepository
    {
        public Task<List<Style>> GetAllStylesAsync();
        public Task<List<Style>> GetAllStylesActiveAsync();
        public Task<Style> GetStyleAcync(int id);
        public Task<Style> AddStyleAsync(Style style);
        public Task UpdateStyleAsync(Style style);
        public Task DeleteStyleAsync(int id);
        public Task<Style> GetStyleByNameAcync(string name);
    }
}
