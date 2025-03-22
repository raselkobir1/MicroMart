using Microsoft.EntityFrameworkCore;

namespace User.API.DataAccess.DataContext
{
    public class ApplicationDbContext: CommonDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
