using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMqPostman.BLL.Interfaces;
using RabbitMqPostman.BLL.Models;
using RabbitMqPostman.BLL.RebitMq;
using RabbitMqPostman.BLL.v1.Handlers;
using RabbitMqPostman.BLL.v1.Handlers.Comands;
using RabbitMqPostman.BLL.v1.Services;

namespace RabbitMqPostman.BLL
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
            {//todo включить мониторинг ребит
             //   services.AddHostedService<RabbitMqNewCustomerListener>();
            }

            services.AddTransient<IRebitMqSender, RabitMqSender>();

            //for mediatoR
            services.AddTransient<IRequestHandler<CreateCustomerCommand, Customer>, CreateCustomerCommandHandler>();
        }
    }
}
