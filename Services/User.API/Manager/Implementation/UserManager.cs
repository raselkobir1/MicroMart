using User.API.DataAccess.UnitOfWorks;
using User.API.DataAcess.DataContext;
using User.API.Domain.Dto.Common;
using User.API.Domain.Dtos;
using User.API.Helper;
using User.API.Manager.Interface;
using Mapster;
//using User.API.Helper.Client;

namespace User.API.Manager.Implementation
{
    public class UserManager : IUserManager
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly InventoryServiceClient _inventoryClient;
        public UserManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            //_inventoryClient = inventoryClient;
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

            if (await _unitOfWork.Users.Any(x => x.AuthUserId == dto.AuthUserId))
                return Utilities.ValidationErrorResponse("Auth user already exists");

            var User = dto.Adapt<Domain.Entities.User>();
            User.Status = Helper.Enums.UserStatus.PENDING;
            User.SetCommonPropertiesForCreate(_unitOfWork.GetLoggedInUserId());
            try
            {
                _unitOfWork.Users.Add(User);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }


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
