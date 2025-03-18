using Inventory.API.DataAccess.UnitOfWorks;

namespace Inventory.API
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IEmailService, EmailService>();
        }
    }
}
