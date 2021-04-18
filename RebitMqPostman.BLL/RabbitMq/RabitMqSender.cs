using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMqPostman.BLL.Interfaces;
using RabbitMqPostman.BLL.Models;
using RabbitMqPostman.Common.Infrastructure;
using RabbitMqPostman.Common.Models;

namespace RabbitMqPostman.BLL.RebitMq
{
    public class RabitMqSender : IRebitMqSender
    {
        private IConnection _connection;
        private readonly ILogger<RabitMqSender> _logger;
        private readonly RabbitMqConfiguration _rabbitMqConfiguration;
        private readonly RequestInfo _requestInfo;

        public RabitMqSender(ILogger<RabitMqSender> logger, IOptions<RabbitMqConfiguration> rabbitMqOptions,
                             IOptions<RequestInfo> requestInfo)
        {
            _logger = logger;
            _requestInfo = requestInfo.Value;
            _rabbitMqConfiguration = rabbitMqOptions.Value;

            CreateConnection();
        }

        public void SendMessage(object message)
        {
            _logger.LogDebug(_requestInfo.CorrelationId, "RebitMq start send message");

            if (ConnectionExists() && _rabbitMqConfiguration.Enabled)
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _rabbitMqConfiguration.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: _rabbitMqConfiguration.QueueName, basicProperties: null, body: body);

                    _logger.LogDebug(_requestInfo.CorrelationId, "RebitMq send message");
                }
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _rabbitMqConfiguration.Hostname,
                    UserName = _rabbitMqConfiguration.UserName,
                    Password = _rabbitMqConfiguration.Password
                };

                _connection = factory.CreateConnection();

                _logger.LogDebug(_requestInfo.CorrelationId, "Connection created");
            }
            catch (Exception ex)
            {
                _logger.LogError(_requestInfo.CorrelationId, ex, "Could not create connection");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                _logger.LogDebug(_requestInfo.CorrelationId, "Connection exist");

                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}
