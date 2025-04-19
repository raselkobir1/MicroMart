namespace Cart.API.Manager
{
    using Cart.API.Domain.Dto.Common;
    using Cart.API.Domain.Dtos;
    using Cart.API.Helper;
    using Cart.API.Helper.Client;
    using StackExchange.Redis;
    using System.Text.Json;

    public class RedisCartService : IRedisCartService
    {
        private readonly IDatabase _redisDb;
        private const int SessionTtlSeconds = 600;
        private readonly InventoryServiceClient _inventoryClient;
        public RedisCartService(IConnectionMultiplexer connectionMultiplexer, InventoryServiceClient serviceClient)
        {
            var _mux = connectionMultiplexer;
            _redisDb = _mux.GetDatabase();
            _inventoryClient = serviceClient;
        }

        private static string GetCartKey(string sessionId) => $"cart:{sessionId}";
        private static string GetSessionKey(string sessionId) => $"sessions:{sessionId}";

        public async Task<ResponseModel> AddToCartAsync(string? sessionId, CartItemDto item)
        {
            if (string.IsNullOrWhiteSpace(item.ProductId) || string.IsNullOrWhiteSpace(item.InventoryId) || item.Quantity <= 0)
                return Utilities.ValidationErrorResponse("Invalid cart item");

            if (!string.IsNullOrEmpty(sessionId))
            {
                var exists = await _redisDb.KeyExistsAsync(GetSessionKey(sessionId));
                if (!exists) sessionId = null;
            }
            // If sessionId is still null, create a new one
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                await _redisDb.StringSetAsync(GetSessionKey(sessionId), sessionId, TimeSpan.FromSeconds(SessionTtlSeconds));
            }

            // Get inventory quentity:
            var inventory = await _inventoryClient.GetInventoryById(Convert.ToInt64(item.InventoryId));

            if (!inventory.IsSuccess)
                return Utilities.ValidationErrorResponse("Failed to fetch inventory");

            int stock = inventory.Data.Quantity;
            if (item.Quantity > stock)
                return Utilities.ValidationErrorResponse("Not enough inventory");

            // Add or update cart item
            var cartKey = GetCartKey(sessionId);
            var existing = await _redisDb.HashGetAsync(cartKey, item.ProductId);
            CartItemDto? existingItem = existing.HasValue ? JsonSerializer.Deserialize<CartItemDto>(existing) : null;
 
            int totalQty = item.Quantity + (existingItem?.Quantity ?? 0);

            var updatedItem = new CartItemDto
            {
                ProductId = item.ProductId,
                InventoryId = item.InventoryId,
                Quantity = totalQty
            };

            await _redisDb.HashSetAsync(cartKey, item.ProductId, JsonSerializer.Serialize(updatedItem));
            //await _redisDb.KeyExpireAsync(cartKey, TimeSpan.FromSeconds(SessionTtlSeconds));
            await _redisDb.KeyExpireAsync(GetSessionKey(sessionId), TimeSpan.FromSeconds(SessionTtlSeconds));

            // Update inventory quentity:
            var invObject = new
            {
                Id = inventory.Data.Id,
                ActionType = 2, // 2 = OUT
                ProductId = inventory.Data.ProductId,
                Name = inventory.Data.Name,
                SKU = inventory.Data.SKU,
                Description = inventory.Data.Description,
                Quantity = item.Quantity,
            };

            var inv =await _inventoryClient.UpdateInventoryAsync(invObject);

            return Utilities.SuccessResponseForAdd(sessionId);
        }


        public async Task<ResponseModel> GetCartItemsAsync(string sessionId)
        {
            var cartKey = GetCartKey(sessionId);
            var hash = await _redisDb.HashGetAllAsync(cartKey);

            var result = new Dictionary<string, CartItemDto>();
            foreach (var entry in hash)
            {
                var item = JsonSerializer.Deserialize<CartItemDto>(entry.Value!);
                if (item != null)
                    result[entry.Name!] = item;
            }

            return Utilities.SuccessResponseForGet(result);
        }

        public async Task<ResponseModel> RemoveCartItemAsync(string sessionId, string productId)
        {
            var cartKey = GetCartKey(sessionId);
            var sessionKey = GetSessionKey(sessionId);
            await _redisDb.KeyDeleteAsync(sessionKey);
            var result = await _redisDb.HashDeleteAsync(cartKey, productId);

            if (!result)
                return Utilities.ValidationErrorResponse("Item not found in cart");
            return Utilities.SuccessResponseForDelete(); 
        }

        public async Task<ResponseModel> RemoveCartAsync(string sessionId)
        {
            var sessionKey = GetSessionKey(sessionId);
            var cartKey = GetCartKey(sessionId);
            bool isCartRemoved = await _redisDb.KeyDeleteAsync(cartKey);
            bool isSessionRemoved = await _redisDb.KeyDeleteAsync(sessionKey); 
            if(isCartRemoved && isSessionRemoved)
            {
                return Utilities.SuccessResponseForDelete();
            }
            else
            {
                return Utilities.ValidationErrorResponse("Not deleted item or cart");
            }
        }
    }
}
