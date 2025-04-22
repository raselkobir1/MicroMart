using Order.API.DataAccess.UnitOfWorks;
using Order.API.Domain.Dto.Common;
using Order.API.Domain.Dtos;
using Order.API.Manager.Interface;
using Order.API.Helper.Client;
using Order.API.Helper;
using Order.API.Helper.Enums;
using Order.API.MessageBroker;
using Microsoft.Extensions.Options;
using Order.API.Domain.Dtos.Common;

namespace Order.API.Manager.Implementation
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailServiceClient _emailClient;
        private readonly CartServiceClient _cartClient;
        private readonly ProductServiceClient _productClient;
        private readonly IRabbitMQMessageProducer _rabbitMQProducer;
        private readonly RabbitMqSettings _rabbitMQettings;
        public OrderManager(IUnitOfWork unitOfWork, EmailServiceClient emailClient, CartServiceClient cartClient, ProductServiceClient productClient, IRabbitMQMessageProducer rabbitMQProducer, IOptions<RabbitMqSettings> settings)
        {
            _unitOfWork = unitOfWork;
            _emailClient = emailClient;
            _cartClient = cartClient;
            _productClient = productClient;
            _rabbitMQProducer = rabbitMQProducer;
            _rabbitMQettings = settings.Value;
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

            #region Validation
            var validationResult = new OrderAddDtoValidator().Validate(dto);

            if (!validationResult.IsValid)
                return Utilities.ValidationErrorResponse(CommonMethods.ConvertFluentErrorMessages(validationResult.Errors));
            #endregion

            var apiResponse = await _cartClient.GetCartItemsAsync(dto.CartSessionId);
            if (!apiResponse.IsSuccess)
                return Utilities.ValidationErrorResponse("Cart items empty.");

            var cartItems = apiResponse.Data.Values.ToList();
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
            var sendEmail = new EmailSendDto();
            sendEmail.To.Add(dto.UserEmail);
            sendEmail.Body = $"Hello, {dto.UserName} \nThank you, Successfully completed your order. \nyour orderId is: {order.Id}";
            sendEmail.Subject = "Order succesfully completed";
            //await _emailClient.SendEmailAsync(sendEmail);
            await _rabbitMQProducer.SendMessageToQueue(_rabbitMQettings.EmailQueueName, sendEmail);
            // clear cart
            //await _cartClient.RemoveCartBySessionId(dto.CartSessionId);
            await _rabbitMQProducer.SendMessageToQueue(_rabbitMQettings.ClearCartQueueName,  dto.CartSessionId );

            return Utilities.SuccessResponseForAdd(dto);
        }

        public async Task<ResponseModel> OrderGetAll(OrderFilterDto dto)
        {
            #region Validation
            var validationResult = new OrderFilterDtoValidator().Validate(dto); 
            if (!validationResult.IsValid)
                return Utilities.ValidationErrorResponse(CommonMethods.ConvertFluentErrorMessages(validationResult.Errors));
            #endregion

            var result = await _unitOfWork.Orders.GetPasignatedResult(dto);
            return Utilities.SuccessResponseForGet(result);
        }

        public async Task<ResponseModel> OrderGetById(long id)
        {
            var order = await _unitOfWork.Orders.GetWhere(x=> x.Id == id, x=> x.OrderItems);
            return Utilities.SuccessResponseForAdd(order);
        }
    }
}
