using Microsoft.EntityFrameworkCore;

namespace Auth.API.DataAccess.DataContext
{
    public interface ICommonDbContext
    {
        DbSet<Domain.Entities.User> Users { get; set; }  
        //DbSet<InventoryHistory> InventoryHistory { get; set; }   
    }
}
