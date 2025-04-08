using Auth.API.Domain.Dtos;
using Auth.API.Domain.Dtos.PaginatedResult;
using Auth.API.Domain.Entities;

namespace Auth.API.DataAccess.Interfaces
{
    public interface IUserRepository: IGenericRepository<Domain.Entities.User>
    {
        Task<PagingResponseDto> GetPasignatedResult(UserFilterDto dto);
        Task RemoveUser(User user);
    }
}
