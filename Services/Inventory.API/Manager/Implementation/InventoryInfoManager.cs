using Inventory.API.Domain.Dto.Common;
using Inventory.API.Domain.Dtos;
using Inventory.API.Manager.Interface;

namespace Inventory.API.Manager.Implementation
{
    public class InventoryInfoManager : IInventoryInfoManager
    {
        public Task<ResponseModel> GetDropdownForInventor()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> InventoryInfoAdd(InventoryInfoAddDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> InventoryInfoDelete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> InventoryInfoGetAll(InventoryInfoFilterDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> InventoryInfoGetById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> InventoryInfoUpdate(InventoryInfoUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
