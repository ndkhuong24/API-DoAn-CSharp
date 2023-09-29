using API.Data;

namespace API.Repository
{
    public interface IStyleRepository
    {
        public Task<List<Style>> GetAllStylesAsync();
        public Task<Style> GetStylesAsync(int id);
        public Task<int> AddStyleAsync(Style style);
        public Task UpdateStyleAsync(int id, Style style);
        public Task DeleteStyleAsync(int id);
    }
}
