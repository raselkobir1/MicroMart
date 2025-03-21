using Microsoft.EntityFrameworkCore;

namespace Product.API.DataAccess.DataContext
{
    public interface ICommonDbContext
    {
        DbSet<Domain.Entities.Product> Products { get; set; } 
        //DbSet<InventoryHistory> InventoryHistory { get; set; }   
    }
}
