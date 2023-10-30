using API.Data;

namespace API.Repository
{
    public interface IStyleRepository
    {
        public Task<List<Style>> GetAllStylesAsync();
        public Task<Style> GetStylesAsync(int id);
        public Task<Style> AddStyleAsync(Style style);
        public Task UpdateStyleAsync(Style style);
        public Task DeleteStyleAsync(int id);
    }
}
