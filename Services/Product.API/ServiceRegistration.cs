using Product.API.DataAccess.UnitOfWorks;
using Product.API.Manager.Implementation;
using Product.API.Manager.Interface;

namespace Product.API
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductManager, ProductManager>();
        }
    }
}
