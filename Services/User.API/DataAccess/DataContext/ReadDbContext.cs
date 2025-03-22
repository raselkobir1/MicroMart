using Microsoft.EntityFrameworkCore;

namespace User.API.DataAccess.DataContext
{
    public class ReadDbContext : CommonDbContext
    {
        public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
        {
        }
    }
}
