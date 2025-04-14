using Cart.API.Domain.Dto.Common;
using Cart.API.Domain.Dtos;

namespace Cart.API.Manager
{
    public interface IRedisCartService
    {
        Task<ResponseModel> AddToCartAsync(string? sessionId, CartItemDto item);
        Task<Dictionary<string, CartItemDto>> GetCartItemsAsync(string sessionId);
        Task<bool> UpdateCartItemAsync(string sessionId, string productId, int quantity);
        Task<bool> RemoveCartItemAsync(string sessionId, string productId);
    }
}
