
using StackExchange.Redis;

namespace Cart.API.Helper.Extensions
{
    public static class PersistenceServiceExtension
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            var options = ConfigurationOptions.Parse(configuration.GetConnectionString("RedisConnection") ?? "localhost,ssl=False,abortConnect=False");
            //options.KeepAlive = 60; // keep connections alive for 60 seconds
            //options.ClientName = "CartService";
            //options.DefaultDatabase = 0;
            options.AllowAdmin = true; // Enable Redis commands like CONFIG
            var redisConnection = ConnectionMultiplexer.Connect(options);

            services.AddSingleton<IConnectionMultiplexer>(redisConnection);

            return services;
        }
    }
}

