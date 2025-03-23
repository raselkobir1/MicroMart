using Auth.API.DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Helper.Extensions
{
    public static class PersistenceServiceExtension
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), options =>
                {
                    options.MigrationsHistoryTable("__EFMigrationsHistory", "user");
                })
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );

            services.AddDbContext<ReadDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("ReadDatabaseConnection"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );

            //var redisConnection = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection") ?? "localhost,ssl=False,abortConnect=False");
            //services.AddSingleton<IConnectionMultiplexer>(redisConnection);

            return services;
        }
    }
}

