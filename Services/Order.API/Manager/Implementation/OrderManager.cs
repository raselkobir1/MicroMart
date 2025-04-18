using Order.API.DataAccess.UnitOfWorks;
using Order.API.Domain.Dto.Common;
using Order.API.Domain.Dtos;
using Order.API.Manager.Interface;
using Order.API.Helper.Client;
using Order.API.Helper;
using Order.API.Helper.Enums;

namespace Order.API.Manager.Implementation
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailServiceClient _inventoryClient;
        private readonly CartServiceClient _cartClient;
        private readonly ProductServiceClient _productClient;
        public OrderManager(IUnitOfWork unitOfWork, EmailServiceClient inventoryClient, CartServiceClient cartClient, ProductServiceClient productClient)
        {
            _unitOfWork = unitOfWork;
            _inventoryClient = inventoryClient;
            _cartClient = cartClient;
            _productClient = productClient;
        }

        public async Task<ResponseModel> OrderCheckout(OrderAddDto dto)
        {
            var order = new Domain.Entities.Order
            {
                Status = OrderStatus.CONFIRMED,
                UserId = dto.UserId.Value,
                UserName = dto.UserName,
                UserEmail = dto.UserEmail,
                OrderItems = new List<Domain.Entities.OrderItem>()
            };
            // checkoutProcess:
            //1.validate user input
            //2.get cart items using cart sessionId
            //3. if cart is empty retutn 400 error.
            //4.find all product details by the product id from carts.
            //5.invoke email service.
            //6.invoke cart service.
            #region Validation
            var validationResult = new OrderAddDtoValidator().Validate(dto);

            if (!validationResult.IsValid)
                return Utilities.ValidationErrorResponse(CommonMethods.ConvertFluentErrorMessages(validationResult.Errors));
            #endregion

            var apiResponse = await _cartClient.GetCartItemsAsync(dto.CartSessionId);
            if (!apiResponse.IsSuccess)
                return Utilities.ValidationErrorResponse("Cart items empty.");

            var cartItems = apiResponse.Data;
            foreach (var cartItem in cartItems) 
            { 
                var productResponse = await _productClient.GetProductAsync(cartItem.ProductId);
                if (productResponse.IsSuccess)
                {
                    order.OrderItems.Add(new Domain.Entities.OrderItem
                    {
                        ProductId = Convert.ToInt64( cartItem.ProductId),
                        InventoryId = Convert.ToInt64(cartItem.InventoryId),
                        Quantity = cartItem.Quantity,
                        Price = productResponse.Data.Price,
                        ProductName = productResponse.Data.Name,
                        SKU = productResponse.Data.SKU,
                        Total = productResponse.Data.Price,
                    });
                }
            }

            if (order.OrderItems.Count == 0)
                return Utilities.ValidationErrorResponse("No product found.");

             _unitOfWork.Orders.Add(order);
            await _unitOfWork.SaveAsync();
            // send email

            return Utilities.SuccessResponseForAdd(dto);
        }
    }
}
