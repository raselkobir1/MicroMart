using Order.API.Domain.Dto.Common;
using Order.API.Domain.Dtos;

namespace Order.API.Manager.Interface 
{
    public interface IOrderManager  
    {
        Task<ResponseModel> OrderCheckout(OrderAddDto dto);
        //Task<ResponseModel> OrderDelete(long id);
        Task<ResponseModel> OrderGetById(long id);
        Task<ResponseModel> OrderGetAll(OrderFilterDto dto); 
    }
}
