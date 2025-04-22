using Order.API.DataAccess.UnitOfWorks;
using Order.API.Domain.Dtos.Common;
using Order.API.Manager.Implementation;
using Order.API.Manager.Interface;
using Order.API.MessageBroker;

namespace Product.API
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddOptions<RabbitMqSettings>().BindConfiguration(nameof(RabbitMqSettings)).ValidateDataAnnotations();
            services.AddScoped<IRabbitMQMessageProducer, RabbitMQMessageProducer>();
        }
    }
}
