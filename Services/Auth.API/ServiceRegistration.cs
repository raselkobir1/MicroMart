using Auth.API.DataAccess.UnitOfWorks;
using Auth.API.Domain.Dtos.Common;
using Auth.API.Helper.Configuration;
using Auth.API.Manager.Implementation;
using Auth.API.Manager.Interface;
using Auth.API.MessageBroker;

namespace Auth.API
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<ILoginManager, LoginManager>();
            services.AddOptions<RabbitMqSettings>().BindConfiguration(nameof(RabbitMqSettings)).ValidateDataAnnotations();
            services.AddScoped<IRabbitMQMessageProducer, RabbitMQMessageProducer>();

            #region Options Config
            services.AddOptions<JwtTokenConfiguration>().BindConfiguration(nameof(JwtTokenConfiguration)).ValidateDataAnnotations();
            #endregion
        }
    }
}
