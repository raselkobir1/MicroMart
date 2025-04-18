namespace Cart.API.Helper.Job
{
    using Cart.API.Domain.Dtos;
    using Cart.API.Helper.Client;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using StackExchange.Redis;
    using System.Text.Json;

    public class RedisKeyExpirationListener : BackgroundService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisKeyExpirationListener> _logger;
        private readonly InventoryServiceClient _inventoryClient;
        public RedisKeyExpirationListener(IConnectionMultiplexer redis, ILogger<RedisKeyExpirationListener> logger, InventoryServiceClient client)
        {
            _redis = redis; 
            _logger = logger;
            _inventoryClient = client;
        }
        private static string GetCartKey(string sessionId) => $"cart:{sessionId}";
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _redis.GetSubscriber();
            var endpoint = _redis.GetEndPoints().First();
            var server = _redis.GetServer(endpoint);

            // Ensure Redis is configured to publish key expiration events
            await server.ConfigSetAsync("notify-keyspace-events", "Ex");

            _logger.LogInformation("Subscribing to Redis key expiration events...");

            await subscriber.SubscribeAsync("__keyevent@0__:expired", async (channel, key) =>
            {
                _logger.LogWarning($"[Redis Expired] Key: {key}");
                _logger.LogWarning($"[Redis Expired] Channel: {channel}");

                 var sessionId = key.ToString().Split(":").Last();
                var cartKey = GetCartKey(sessionId);
                var cartItems = await _redis.GetDatabase().HashGetAllAsync(cartKey);
                foreach (var entry in cartItems)
                {
                    var item = JsonSerializer.Deserialize<CartItemDto>(entry.Value!);
                    var inventory = await _inventoryClient.GetInventoryById(Convert.ToInt64(item.InventoryId));
                    if (item != null)
                    {
                        var invObject = new
                        {
                            Id = item.InventoryId,
                            ActionType = 1, // 1 = IN
                            ProductId = item.ProductId,
                            Name = inventory.Data.Name,
                            SKU = inventory.Data.SKU,
                            Description = inventory.Data.Description,
                            Quantity = item.Quantity,
                        };

                        var inv = await _inventoryClient.UpdateInventoryAsync(invObject);
                    }
                }
                bool deleted = await _redis.GetDatabase().KeyDeleteAsync(cartKey);
            });
        }
    }
}
