using Microsoft.EntityFrameworkCore;

namespace Order.API.DataAccess.DataContext
{
    public interface ICommonDbContext
    {
        DbSet<Domain.Entities.Order> Orders { get; set; } 
        DbSet<Domain.Entities.OrderItem> OrderItems { get; set; }  
    }
}
