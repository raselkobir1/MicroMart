using User.API.Domain.Dto.Common;
using User.API.Domain.Dtos;

namespace User.API.Manager.Interface 
{
    public interface IUserManager  
    {
        Task<ResponseModel> UserAdd(UserAddDto dto);
        Task<ResponseModel> UserUpdate(UserUpdateDto dto);
        Task<ResponseModel> UserDelete(long id);
        Task<ResponseModel> UserGetById(long id);
        Task<ResponseModel> UserGetAll(UserFilterDto dto);
        Task<ResponseModel> GetDropdownForInventor(); 
    }
}
