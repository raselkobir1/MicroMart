using Product.API.Domain.Dto.Common;
using Product.API.Domain.Dtos;

namespace Product.API.Manager.Interface 
{
    public interface IProductManager  
    {
        Task<ResponseModel> ProductAdd(ProductAddDto dto);
        //Task<ResponseModel> ProductUpdate(ProductUpdateDto dto);
        Task<ResponseModel> ProductDelete(long id);
        Task<ResponseModel> ProductGetById(long id);
        Task<ResponseModel> ProductGetAll(ProductFilterDto dto);
        Task<ResponseModel> GetDropdownForInventor(); 
    }
}
