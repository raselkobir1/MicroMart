using Microsoft.EntityFrameworkCore;

namespace Auth.API.DataAccess.DataContext
{
    public class ReadDbContext : CommonDbContext
    {
        public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
        {
        }
    }
}
