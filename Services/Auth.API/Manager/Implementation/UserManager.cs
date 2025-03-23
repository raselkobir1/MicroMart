using Auth.API.DataAccess.UnitOfWorks;
using Auth.API.DataAcess.DataContext;
using Auth.API.Domain.Dto.Common;
using Auth.API.Domain.Dtos;
using Auth.API.Helper;
using Auth.API.Manager.Interface;
using Mapster;
using Auth.API.Helper.Client;
using BCrypt.Net;

namespace Auth.API.Manager.Implementation
{
    public class UserManager : IUserManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserProfileServiceClient _userProfileClient;
        public UserManager(IUnitOfWork unitOfWork, UserProfileServiceClient userProfileClient)
        {
            _unitOfWork = unitOfWork;
            _userProfileClient = userProfileClient; 
        }
        public Task<ResponseModel> GetDropdownForInventor()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> UserAdd(UserAddDto dto)
        {
            #region Validation
            var validationResult = new UserAddDtoValidator().Validate(dto);

            if (!validationResult.IsValid)
                return Utilities.ValidationErrorResponse(CommonMethods.ConvertFluentErrorMessages(validationResult.Errors));

            #endregion

            if (await _unitOfWork.Users.Any(x => x.Email.Trim() == dto.Email.Trim()))
                return Utilities.ValidationErrorResponse("Auth user email already exists");

            if (await _unitOfWork.Users.Any(x => x.UserName.Trim() == dto.UserName.Trim()))
                return Utilities.ValidationErrorResponse("Auth user name already exists");

            var passHash = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password, HashType.SHA512, workFactor: 13);
            var User = dto.Adapt<Domain.Entities.User>();
            User.Password = passHash;  
            User.Status = Helper.Enums.AccountStatus.PENDING;
            User.Role = Helper.Enums.Roles.Admin;
            User.Verified = false;
            User.SetCommonPropertiesForCreate(_unitOfWork.GetLoggedInUserId());

            _unitOfWork.Users.Add(User);
            await _unitOfWork.SaveAsync();

            // create user profile
            var result = await _userProfileClient.CreateUserProfileAsync(User);
            if (!result)
            {
                return Utilities.ValidationErrorResponse("Failed to create user profile");
            }
            // Generate verification code and save in table: verificationCode

            // Send email to user for verification

            var finalResponse = User.Adapt<UserAddDto>();
            return Utilities.SuccessResponseForAdd(finalResponse);
        }

        public async Task<ResponseModel> UserDelete(long id)
        {
            var User = await _unitOfWork.Users.GetById(id);
            if (User == null)
                return Utilities.NotFoundResponse("User not found");

            User.IsDeleted = true;
            User.SetCommonPropertiesForUpdate(_unitOfWork.GetLoggedInUserId());
            _unitOfWork.Users.Update(User);
            await _unitOfWork.SaveAsync();

            return Utilities.SuccessResponseForDelete();
        }

        public async Task<ResponseModel> UserGetAll(UserFilterDto dto)
        {
            #region Validation
            var validationResult = new UserFilterDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
                return Utilities.ValidationErrorResponse(CommonMethods.ConvertFluentErrorMessages(validationResult.Errors));
            #endregion

            var result = await _unitOfWork.Users.GetPasignatedResult(dto);
            return Utilities.SuccessResponseForGet(result);
        }

        public async Task<ResponseModel> UserGetById(long id)
        {
            var result = await _unitOfWork.Users.GetById(id);
            if (result == null)
                return Utilities.NotFoundResponse("User not found");
            return Utilities.SuccessResponseForGet(result);
        }

        public async Task<ResponseModel> UserUpdate(UserUpdateDto dto)
        {
            #region Validation
            var validationResult = new UserUpdateDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
                return Utilities.ValidationErrorResponse(CommonMethods.ConvertFluentErrorMessages(validationResult.Errors));

            if (await _unitOfWork.Users.Any(x => x.Email.Trim() == dto.Email.Trim() && x.Id != dto.Id))
                return Utilities.ValidationErrorResponse("Auth user email already exists");

            if (await _unitOfWork.Users.Any(x => x.UserName.Trim() == dto.UserName.Trim() && x.Id != dto.Id))
                return Utilities.ValidationErrorResponse("Auth user name already exists");
            #endregion

            var User = await _unitOfWork.Users.GetById(dto.Id);
            if (User == null)
                return Utilities.NotFoundResponse("User not found");

            User = dto.Adapt(User);
            User.SetCommonPropertiesForUpdate(_unitOfWork.GetLoggedInUserId());
            _unitOfWork.Users.Update(User);

            await _unitOfWork.SaveAsync();

            var finalResponse = User.Adapt<UserAddDto>();
            return Utilities.SuccessResponseForAdd(finalResponse);
        }
    }
}
