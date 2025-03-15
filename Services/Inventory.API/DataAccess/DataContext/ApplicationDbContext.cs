using Microsoft.EntityFrameworkCore;

namespace Inventory.API.DataAccess.DataContext
{
    public class ApplicationDbContext: CommonDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
