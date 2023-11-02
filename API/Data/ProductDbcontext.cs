using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ProductDbcontext : DbContext
    {
        public ProductDbcontext(DbContextOptions<ProductDbcontext> options) : base(options)
        {
        }
        public DbSet<Product> Product { get; set; }
    }
}
