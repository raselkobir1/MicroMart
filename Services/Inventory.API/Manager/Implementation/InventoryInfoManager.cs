using FluentValidation;
using Inventory.API.DataAccess.UnitOfWorks;
using Inventory.API.DataAcess.DataContext;
using Inventory.API.Domain.Dto.Common;
using Inventory.API.Domain.Dtos;
using Inventory.API.Domain.Entities;
using Inventory.API.Helper;
using Inventory.API.Helper.Enums;
using Inventory.API.Helper.Resources;
using Inventory.API.Manager.Interface;
using Mapster;
using System.Data;

namespace Inventory.API.Manager.Implementation
{
    public class InventoryInfoManager : IInventoryInfoManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public InventoryInfoManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<ResponseModel> GetDropdownForInventor()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> InventoryInfoAdd(InventoryInfoAddDto dto)
        {
            #region Validation
            var validationResult = new InventoryInfoAddDtoValidator().Validate(dto);

            if (!validationResult.IsValid)
                return Utilities.ValidationErrorResponse(CommonMethods.ConvertFluentErrorMessages(validationResult.Errors));

            #endregion

            var inventory = dto.Adapt<InventoryInfo>();
            inventory.SKU = dto.SKU;
            inventory.InventoryHistory = new List<InventoryHistory>
            {
                new InventoryHistory
                {
                     SKU = dto.SKU,
                     ActionType = ActionType.IN,
                     LastQuentity = 0,
                     NewQuentity = dto.Quantity,
                     QuentityChanged = 0,
                }
            };
            inventory.SetCommonPropertiesForCreate(_unitOfWork.GetLoggedInUserId());
            inventory.InventoryHistory.ForEach(x => x.SetCommonPropertiesForCreate(_unitOfWork.GetLoggedInUserId()));
            _unitOfWork.InventoryInfos.Add(inventory);
            await _unitOfWork.SaveAsync();

            var finalResponse = inventory.Adapt<InventoryInfoAddDto>();
            return Utilities.SuccessResponseForAdd(finalResponse);
        }

        public async Task<ResponseModel> InventoryInfoDelete(long id)
        {
            var inventory = await _unitOfWork.InventoryInfos.GetById(id);
            if (inventory == null)
                return Utilities.NotFoundResponse("Inventory not found");

            inventory.IsDeleted = true;
            inventory.SetCommonPropertiesForUpdate(_unitOfWork.GetLoggedInUserId());
            _unitOfWork.InventoryInfos.Update(inventory);
            await _unitOfWork.SaveAsync();

            return Utilities.SuccessResponseForDelete();
        }

        public async Task<ResponseModel> InventoryInfoGetAll(InventoryInfoFilterDto dto)
        {
            #region Validation
            var validationResult = new InventoryInfoFilterDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
                return Utilities.ValidationErrorResponse(CommonMethods.ConvertFluentErrorMessages(validationResult.Errors));
            #endregion

            var result = await _unitOfWork.InventoryInfos.GetPasignatedResult(dto);
            return Utilities.SuccessResponseForGet(result);
        }

        public async Task<ResponseModel> InventoryInfoGetById(long id)
        {
            var result = await _unitOfWork.InventoryInfos.GetById(id);
            if (result == null)
                return Utilities.NotFoundResponse("Inventory not found");
            return Utilities.SuccessResponseForGet(result);
        }

        public async Task<ResponseModel> InventoryInfoUpdate(InventoryInfoUpdateDto dto)
        {
            #region Validation
            var validationResult = new InventoryInfoUpdateDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
                return Utilities.ValidationErrorResponse(CommonMethods.ConvertFluentErrorMessages(validationResult.Errors));
            #endregion

            var inventory = await _unitOfWork.InventoryInfos.GetById(dto.Id);
            if (inventory == null)
                return Utilities.NotFoundResponse("Inventory not found");

            var lastHistory = await _unitOfWork.InventoryInfos.GetLastInventoryHistoryByInventoryId(inventory.Id);
            if (lastHistory == null)
                return Utilities.NotFoundResponse("Inventory history not found");

            int currentQuentity = inventory.Quantity;
            if (dto.ActionType == ActionType.IN)
                currentQuentity += dto.Quantity;
            else if(dto.ActionType == ActionType.OUT)
                currentQuentity -= dto.Quantity;

            inventory = dto.Adapt(inventory);
            inventory.Quantity = currentQuentity;

            var invHistory = new InventoryHistory
                             {
                                InventoryInfoId = inventory.Id,
                                ActionType = dto.ActionType,
                                QuentityChanged = dto.Quantity,
                                LastQuentity = lastHistory.NewQuentity,
                                NewQuentity = currentQuentity,
                                SKU = inventory.SKU,
                             };

            inventory.SetCommonPropertiesForUpdate(_unitOfWork.GetLoggedInUserId());
            invHistory.SetCommonPropertiesForCreate(_unitOfWork.GetLoggedInUserId());
            _unitOfWork.InventoryInfos.Update(inventory);
            _unitOfWork.InventoryHistory.Add(invHistory); 

            await _unitOfWork.SaveAsync();

            var finalResponse = inventory.Adapt<InventoryInfoAddDto>();
            return Utilities.SuccessResponseForAdd(finalResponse);
        }
    }
}
