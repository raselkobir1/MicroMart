using Inventory.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.DataAccess.DataContext
{
    public interface ICommonDbContext
    {
        DbSet<InventoryInfo> Inventory { get; set; } 
        DbSet<InventoryHistory> History { get; set; }  
    }
}
