namespace Cart.API.Helper.Job
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using StackExchange.Redis;

    public class RedisKeyExpirationListener : BackgroundService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisKeyExpirationListener> _logger;

        public RedisKeyExpirationListener(IConnectionMultiplexer redis, ILogger<RedisKeyExpirationListener> logger)
        {
            _redis = redis; 
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _redis.GetSubscriber();
            var endpoint = _redis.GetEndPoints().First();
            var server = _redis.GetServer(endpoint);

            // Ensure Redis is configured to publish key expiration events
            await server.ConfigSetAsync("notify-keyspace-events", "Ex");

            _logger.LogInformation("Subscribing to Redis key expiration events...");

            await subscriber.SubscribeAsync("__keyevent@0__:expired", (channel, key) =>
            {
                _logger.LogWarning($"[Redis Expired] Key: {key}");
                _logger.LogWarning($"[Redis Expired] Channel: {channel}");

                // ➕ Add your custom logic here (e.g., clean up, notifications, etc.)
            });
        }
    }
}
