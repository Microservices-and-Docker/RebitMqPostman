using System;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMqPostman.BLL.Interfaces;
using RabbitMqPostman.BLL.Models;
using RabbitMqPostman.Common.Interfaces;
using RabbitMqPostman.Common.Models;

namespace RabbitMqPostman.BLL.RebitMq
{
    public class RabbitMqSender : IRebitMqSender
    {
        private IConnection _connection;
        private readonly IApiLogger _logger;
        private readonly RabbitMqConfiguration _rabbitMqConfiguration;

        public RabbitMqSender(IApiLogger logger, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _logger = logger;
            _rabbitMqConfiguration = rabbitMqOptions.Value;

            CreateConnection();
        }

        public void SendMessage(object message)
        {
            _logger.LogDebug("RebitMq start send message");

            if (ConnectionExists() && _rabbitMqConfiguration.Enabled)
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _rabbitMqConfiguration.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: _rabbitMqConfiguration.QueueName, basicProperties: null, body: body);

                    _logger.LogDebug(message, "RebitMq send message");
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

                _logger.LogDebug(_rabbitMqConfiguration.Hostname, "Connection created");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not create connection");

                throw new ApiException(ErrorCodes.RabbitMq_ConectionError);
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                _logger.LogDebug("Connection exist");

                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}
