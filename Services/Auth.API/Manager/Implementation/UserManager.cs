using Auth.API.DataAccess.UnitOfWorks;
using Auth.API.DataAcess.DataContext;
using Auth.API.Domain.Dto.Common;
using Auth.API.Domain.Dtos;
using Auth.API.Helper;
using Auth.API.Manager.Interface;
using Mapster;
using Auth.API.Helper.Client;
using BCrypt.Net;
using Auth.API.Domain.Entities;
using Auth.API.Helper.Enums;
using Auth.API.Domain.Dtos.Common;

namespace Auth.API.Manager.Implementation
{
    public class UserManager : IUserManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserProfileServiceClient _userProfileClient;
        private readonly SendVerificationEmailClient _sendEmailClient;
        public UserManager(IUnitOfWork unitOfWork, UserProfileServiceClient userProfileClient, SendVerificationEmailClient sendEmailClient)
        {
            _unitOfWork = unitOfWork;
            _userProfileClient = userProfileClient;
            _sendEmailClient = sendEmailClient;
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
            var user = dto.Adapt<Domain.Entities.User>();
            user.Password = passHash;  
            user.Status = Helper.Enums.AccountStatus.PENDING;
            user.Role = Helper.Enums.Roles.USER;
            user.Verified = false;
            user.SetCommonPropertiesForCreate(_unitOfWork.GetLoggedInUserId());

            _unitOfWork.Users.Add(user);
            await _unitOfWork.SaveAsync();

            // create user profile
            var isSuccess = await _userProfileClient.CreateUserProfileAsync(user);
            if (!isSuccess)
            {
                await _unitOfWork.Users.RemoveUser(user);
                return Utilities.ValidationErrorResponse("Failed to create user profile");
            }
            // Generate verification code and save in table: verificationCode
            string verificationCode = CommonMethods.GenerateUniqueRandomNumber().ToString();
            _unitOfWork.VerificationCode.Add(new VerificationCode
            {
                UserId = user.Id,
                Code = verificationCode,
                Status = VerificationStatus.PENDING,
                CreatedBy = _unitOfWork.GetLoggedInUserId(),
                CreatedDate = DateTime.Now,
                ExpiredAt = DateTime.Now.AddMinutes(5),
            });
            await _unitOfWork.SaveAsync();
            // Send email to user for verification
            var emailSendDto = new EmailSendDto
            {
                To = new List<string> { user.Email },
                Subject = "User registration verification Code",
                Body = $"Your verification code is: {verificationCode}",
            };
            var issuccess = await _sendEmailClient.SendVerificationCodeAsync(emailSendDto);
            var finalResponse = user.Adapt<UserAddDto>();
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
