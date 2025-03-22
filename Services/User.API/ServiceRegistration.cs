using User.API.DataAccess.UnitOfWorks;
using User.API.Manager.Implementation;
using User.API.Manager.Interface;

namespace User.API
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
