using Microsoft.EntityFrameworkCore;

namespace Auth.API.DataAccess.DataContext
{
    public class ApplicationDbContext: CommonDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
