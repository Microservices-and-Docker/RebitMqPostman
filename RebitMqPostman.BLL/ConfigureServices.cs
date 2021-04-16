using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RebitMqPostman.BLL.Interfaces;
using RebitMqPostman.BLL.Models;
using RebitMqPostman.BLL.RebitMq;
using RebitMqPostman.BLL.v1.Handlers;
using RebitMqPostman.BLL.v1.Handlers.Comands;
using RebitMqPostman.BLL.v1.Services;

namespace RebitMqPostman.BLL
{
    public static class ConfigureServices
    {
        /// <summary>
        /// Подключение сервисов для RebitMqPostman.BLL
        /// </summary>
        public static void ConfigureBLL(this IServiceCollection services, IConfiguration configuration)
        {
            // services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMqConfiguration"));
            var serviceClientSettingsConfig = configuration.GetSection("RabbitMqConfiguration");
            var serviceClientSettings = serviceClientSettingsConfig.Get<RabbitMqConfiguration>();
            services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);

            if (serviceClientSettings.Enabled)
            {
                services.AddHostedService<RabbitMqNewCustomerListener>();
            }

            services.AddTransient<IRebitMqSender, RabitMqSender>();

            //for mediatoR
            services.AddTransient<IRequestHandler<CreateCustomerCommand, Customer>, CreateCustomerCommandHandler>();
        }
    }
}
