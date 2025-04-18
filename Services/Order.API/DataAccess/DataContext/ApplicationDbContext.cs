using Microsoft.EntityFrameworkCore;

namespace Order.API.DataAccess.DataContext
{
    public class ApplicationDbContext: CommonDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
