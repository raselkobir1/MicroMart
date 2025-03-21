using Product.API.Domain.Dtos;
using Product.API.Domain.Dtos.PaginatedResult;

namespace Product.API.DataAccess.Interfaces
{
    public interface IProductRepository: IGenericRepository<Domain.Entities.Product>
    {
        Task<PagingResponseDto> GetPasignatedResult(ProductFilterDto dto);
        Task RemoveProduct(Domain.Entities.Product product);   
    }
}
