using Microsoft.EntityFrameworkCore;

namespace Product.API.DataAccess.DataContext
{
    public class ApplicationDbContext: CommonDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
