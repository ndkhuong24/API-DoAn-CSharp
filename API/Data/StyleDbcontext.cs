using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class StyleDbcontext : DbContext
    {
        public StyleDbcontext(DbContextOptions<StyleDbcontext> opt) : base(opt)
        {

        }
        public DbSet<Style>? Style { get; set; }
    }
}
