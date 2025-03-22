using User.API.Domain.Dtos;
using User.API.Domain.Dtos.PaginatedResult;

namespace User.API.DataAccess.Interfaces
{
    public interface IUserRepository: IGenericRepository<Domain.Entities.User>
    {
        Task<PagingResponseDto> GetPasignatedResult(UserFilterDto dto);
    }
}
