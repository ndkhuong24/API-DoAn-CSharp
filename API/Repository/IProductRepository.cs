using API.Data;

namespace API.Repository
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllProductAsync();
        public Task<Product> GetProductAsync(int id);
        public Task<Product> AddProductAsync(Product product);
        public Task UpdateProductAsync(Product product);
        public Task DeleteProductAsync(int id);
    }
}
