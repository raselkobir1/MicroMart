using Auth.API.Domain.Dtos;
using Auth.API.Domain.Dtos.PaginatedResult;

namespace Auth.API.DataAccess.Interfaces
{
    public interface IUserRepository: IGenericRepository<Domain.Entities.User>
    {
        Task<PagingResponseDto> GetPasignatedResult(UserFilterDto dto);
    }
}
