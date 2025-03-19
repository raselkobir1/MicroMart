using Inventory.API.Domain.Dto.Common;
using Inventory.API.Domain.Dtos;

namespace Inventory.API.Manager.Interface 
{
    public interface IInventoryInfoManager  
    {
        Task<ResponseModel> InventoryInfoAdd(InventoryInfoAddDto dto);
        Task<ResponseModel> InventoryInfoUpdate(InventoryInfoUpdateDto dto);
        Task<ResponseModel> InventoryInfoDelete(long id);
        Task<ResponseModel> InventoryInfoGetById(long id);
        Task<ResponseModel> InventoryInfoGetAll(InventoryInfoFilterDto dto);
        Task<ResponseModel> GetDropdownForInventor(); 
    }
}
