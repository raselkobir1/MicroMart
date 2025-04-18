using Order.API.DataAccess.UnitOfWorks;
using Order.API.Manager.Implementation;
using Order.API.Manager.Interface;

namespace Product.API
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderManager, OrderManager>();
        }
    }
}
