using Auth.API.DataAccess.UnitOfWorks;
using Auth.API.Manager.Implementation;
using Auth.API.Manager.Interface;

namespace Auth.API
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserManager, UserManager>();
        }
    }
}
