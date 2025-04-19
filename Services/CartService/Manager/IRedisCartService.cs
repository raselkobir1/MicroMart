using Cart.API.Domain.Dto.Common;
using Cart.API.Domain.Dtos;

namespace Cart.API.Manager
{
    public interface IRedisCartService
    {
        Task<ResponseModel> AddToCartAsync(string? sessionId, CartItemDto item);
        Task<ResponseModel> GetCartItemsAsync(string sessionId);
        Task<ResponseModel> RemoveCartItemAsync(string sessionId, string productId);
        Task<ResponseModel> RemoveCartAsync(string sessionId);
    }
}
