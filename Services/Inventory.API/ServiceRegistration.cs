using Inventory.API.DataAccess.UnitOfWorks;
using Inventory.API.Manager.Implementation;
using Inventory.API.Manager.Interface;

namespace Inventory.API
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IInventoryInfoManager, InventoryInfoManager>();
        }
    }
}
