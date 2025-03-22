using Microsoft.EntityFrameworkCore;

namespace User.API.DataAccess.DataContext
{
    public interface ICommonDbContext
    {
        DbSet<Domain.Entities.User> Users { get; set; }  
        //DbSet<InventoryHistory> InventoryHistory { get; set; }   
    }
}
