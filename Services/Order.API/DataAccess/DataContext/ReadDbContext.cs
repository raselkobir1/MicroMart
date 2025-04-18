using Microsoft.EntityFrameworkCore;

namespace Order.API.DataAccess.DataContext
{
    public class ReadDbContext : CommonDbContext
    {
        public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
        {
        }
    }
}
