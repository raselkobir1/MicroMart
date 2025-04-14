
using StackExchange.Redis;

namespace Cart.API.Helper.Extensions
{
    public static class PersistenceServiceExtension
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnection = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection") ?? "localhost,ssl=False,abortConnect=False");
            services.AddSingleton<IConnectionMultiplexer>(redisConnection);

            return services;
        }
    }
}

