using Order.API.Domain.Dtos;
using Order.API.Domain.Dtos.PaginatedResult;

namespace Order.API.DataAccess.Interfaces
{
    public interface IOrderRepository: IGenericRepository<Domain.Entities.Order>
    {
        Task<PagingResponseDto> GetPasignatedResult(OrderFilterDto dto);
    }
}
