using Auth.API.DataAccess.UnitOfWorks;
using Auth.API.Helper.Configuration;
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
            services.AddScoped<ILoginManager, LoginManager>();

            #region Options Config
            services.AddOptions<JwtTokenConfiguration>().BindConfiguration(nameof(JwtTokenConfiguration)).ValidateDataAnnotations();
            #endregion
        }
    }
}
