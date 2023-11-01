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
<<<<<<< HEAD
        public Task<List<Style>> GetSearchNameAsync(string name);
=======
>>>>>>> 1bb5197b9b71aa71b8c158a02259a0b21de7564e
    }
}
