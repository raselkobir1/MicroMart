using Microsoft.EntityFrameworkCore;

namespace Product.API.DataAccess.DataContext
{
    public class ReadDbContext : CommonDbContext
    {
        public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
        {
        }
    }
}
