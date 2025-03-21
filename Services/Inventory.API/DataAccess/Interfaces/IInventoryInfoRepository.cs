using Inventory.API.Domain.Dtos;
using Inventory.API.Domain.Dtos.PaginatedResult;
using Inventory.API.Domain.Entities;

namespace Inventory.API.DataAccess.Interfaces
{
    public interface IInventoryInfoRepository: IGenericRepository<InventoryInfo>
    {
        Task<PagingResponseDto> GetPasignatedResult(InventoryInfoFilterDto dto);
        Task<InventoryHistory> GetLastInventoryHistoryByInventoryId(long inventoryId);  
    }
}
