using Inventory.API.DataAccess.DataContext;
using Inventory.API.DataAccess.Interfaces;
using Inventory.API.Domain.Entities;

namespace Inventory.API.DataAccess.Implementations
{
    public class InventoryHistoryRepository : GenericRepository<InventoryHistory>, IInventoryHistoryRepository
    {
        public InventoryHistoryRepository(ApplicationDbContext context, ReadDbContext readDbContext) : base(context, readDbContext)
        {
        }
    }
}
