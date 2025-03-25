using Auth.WebAPI.Helper.EmailHelper;
using Notification.API.Manager.Interfaces;

namespace User.API
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
