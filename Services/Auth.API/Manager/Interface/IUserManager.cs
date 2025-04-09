using Auth.API.Domain.Dto.Common;
using Auth.API.Domain.Dtos;

namespace Auth.API.Manager.Interface 
{
    public interface IUserManager  
    {
        Task<ResponseModel> UserAdd(UserAddDto dto);
        Task<ResponseModel> UserUpdate(UserUpdateDto dto);
        Task<ResponseModel> UserDelete(long id);
        Task<ResponseModel> UserGetById(long id);
        Task<ResponseModel> UserGetAll(UserFilterDto dto);
        Task<ResponseModel> GetDropdownForInventor();
        Task<ResponseModel> AuthUserVerification(UserVerificationDto dto);
    }
}
