using Inventory.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.DataAccess.DataContext
{
    public interface ICommonDbContext
    {
        DbSet<Product> Products { get; set; } 
    }
}
