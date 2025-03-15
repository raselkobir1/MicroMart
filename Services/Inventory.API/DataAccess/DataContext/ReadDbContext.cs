using Microsoft.EntityFrameworkCore;

namespace Inventory.API.DataAccess.DataContext
{
    public class ReadDbContext : CommonDbContext
    {
        public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
        {
        }
    }
}
