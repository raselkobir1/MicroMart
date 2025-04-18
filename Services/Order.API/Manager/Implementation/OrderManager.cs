using Order.API.DataAccess.UnitOfWorks;
using Order.API.DataAcess.DataContext;
using Order.API.Domain.Dto.Common;
using Order.API.Domain.Dtos;
using Order.API.Helper;
using Order.API.Manager.Interface;
using Mapster;
using Order.API.Helper.Client;

namespace Order.API.Manager.Implementation
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailServiceClient _inventoryClient;
        public OrderManager(IUnitOfWork unitOfWork, EmailServiceClient inventoryClient)
        {
            _unitOfWork = unitOfWork;
            _inventoryClient = inventoryClient;
        }

        public Task<ResponseModel> OrderCheckout(OrderAddDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
