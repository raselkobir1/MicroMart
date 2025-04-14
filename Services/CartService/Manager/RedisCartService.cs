namespace Cart.API.Manager
{
    using Cart.API.Domain.Dto.Common;
    using Cart.API.Domain.Dtos;
    using Cart.API.Helper;
    using StackExchange.Redis;
    using System.Text.Json;

    public class RedisCartService : IRedisCartService
    {
        private readonly IDatabase _redisDb;
        private const int SessionTtlSeconds = 60;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public RedisCartService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _redisDb = _connectionMultiplexer.GetDatabase();
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
            //var client = _httpClientFactory.CreateClient();
            //var invResponse = await client.GetAsync($"https://inventory-service/inventories/{item.InventoryId}");

            //if (!invResponse.IsSuccessStatusCode)
            //    return (false, sessionId, "Failed to fetch inventory");

            //var invJson = JsonDocument.Parse(await invResponse.Content.ReadAsStringAsync());
            //int stock = invJson.RootElement.GetProperty("quantity").GetInt32();
            //if (item.Quantity > stock)
            //    return (false, sessionId, "Not enough inventory");


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
            var result = await _redisDb.HashDeleteAsync(cartKey, productId);
            if (!result)
                return Utilities.ValidationErrorResponse("Item not found in cart");
            return Utilities.SuccessResponseForDelete(); 
        }
    }
}
