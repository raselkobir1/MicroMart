using FluentValidation;
using Product.API.DataAccess.UnitOfWorks;
using Product.API.DataAcess.DataContext;
using Product.API.Domain.Dto.Common;
using Product.API.Domain.Dtos;
using Product.API.Domain.Entities;
using Product.API.Helper;
using Product.API.Helper.Enums;
using Product.API.Helper.Resources;
using Product.API.Manager.Interface;
using Mapster;
using System.Data;
using Product.API.Helper.Client;

namespace Product.API.Manager.Implementation
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly InventoryServiceClient _inventoryClient;
        public ProductManager(IUnitOfWork unitOfWork, InventoryServiceClient inventoryClient)
        {
            _unitOfWork = unitOfWork;
            _inventoryClient = inventoryClient;
        }
        public Task<ResponseModel> GetDropdownForInventor()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> ProductAdd(ProductAddDto dto)
        {
            #region Validation
            var validationResult = new ProductAddDtoValidator().Validate(dto);

            if (!validationResult.IsValid)
                return Utilities.ValidationErrorResponse(CommonMethods.ConvertFluentErrorMessages(validationResult.Errors));

            #endregion

            var product = dto.Adapt<Domain.Entities.Product>();
            product.SetCommonPropertiesForCreate(_unitOfWork.GetLoggedInUserId());

            _unitOfWork.Products.Add(product);
            await _unitOfWork.SaveAsync();

            // create inventory
            bool inventoryCreated = await _inventoryClient.CreateInventoryAsync(product);
            if (!inventoryCreated)
            {
                await _unitOfWork.Products.RemoveProduct(product);
                return Utilities.InternalServerErrorResponse("Failed to create inventory for Product");
            }

            var finalResponse = product.Adapt<ProductAddDto>();
            return Utilities.SuccessResponseForAdd(finalResponse);
        }

        public async Task<ResponseModel> ProductDelete(long id)
        {
            var product = await _unitOfWork.Products.GetById(id);
            if (product == null)
                return Utilities.NotFoundResponse("Product not found");

            product.IsDeleted = true;
            product.SetCommonPropertiesForUpdate(_unitOfWork.GetLoggedInUserId());
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveAsync();

            return Utilities.SuccessResponseForDelete();
        }

        public async Task<ResponseModel> ProductGetAll(ProductFilterDto dto)
        {
            #region Validation
            var validationResult = new ProductFilterDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
                return Utilities.ValidationErrorResponse(CommonMethods.ConvertFluentErrorMessages(validationResult.Errors));
            #endregion

            var result = await _unitOfWork.Products.GetPasignatedResult(dto);
            return Utilities.SuccessResponseForGet(result);
        }

        public async Task<ResponseModel> ProductGetById(long id)
        {
            var result = await _unitOfWork.Products.GetById(id);
            if (result == null)
                return Utilities.NotFoundResponse("Product not found");
            return Utilities.SuccessResponseForGet(result);
        }

        //public async Task<ResponseModel> ProductUpdate(ProductUpdateDto dto)
        //{
        //    #region Validation
        //    var validationResult = new ProductUpdateDtoValidator().Validate(dto);
        //    if (!validationResult.IsValid)
        //        return Utilities.ValidationErrorResponse(CommonMethods.ConvertFluentErrorMessages(validationResult.Errors));
        //    #endregion

        //    var product = await _unitOfWork.Products.GetById(dto.Id);
        //    if (product == null)
        //        return Utilities.NotFoundResponse("Product not found");

        //    var lastHistory = await _unitOfWork.Products.GetLastInventoryHistoryByInventoryId(product.Id);
        //    if (lastHistory == null)
        //        return Utilities.NotFoundResponse("Product history not found");

        //    int newQuentity = product.Quantity;
        //    if (dto.ActionType == ProductStatus.IN)
        //        newQuentity += dto.Quantity;
        //    else
        //        newQuentity -= dto.Quantity;

        //    product = dto.Adapt(product);
        //    product.Quantity = newQuentity;

        //    var invHistory = new InventoryHistory
        //                     {
        //                        ProductId = product.Id,
        //                        ActionType = dto.ActionType,
        //                        QuentityChanged = dto.Quantity,
        //                        LastQuentity = lastHistory.NewQuentity,
        //                        NewQuentity = newQuentity,
        //                        SKU = product.SKU,
        //                     };

        //    product.SetCommonPropertiesForUpdate(_unitOfWork.GetLoggedInUserId());
        //    invHistory.SetCommonPropertiesForCreate(_unitOfWork.GetLoggedInUserId());
        //    _unitOfWork.Products.Update(product);
        //    _unitOfWork.InventoryHistory.Add(invHistory); 

        //    await _unitOfWork.SaveAsync();

        //    var finalResponse = product.Adapt<ProductAddDto>();
        //    return Utilities.SuccessResponseForAdd(finalResponse);
        //}
    }
}
