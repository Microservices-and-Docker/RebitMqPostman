using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

using RebitMqPostman.BLL.Interfaces;
using RebitMqPostman.BLL.Models;
using RebitMqPostman.Common.Logger;

namespace RebitMqPostman.BLL.RebitMq
{
    public class RabitMqSender : IRebitMqSender
    {    
        private IConnection _connection;
        private readonly RabbitMqConfiguration _rabbitMqConfiguration;
        private readonly ILogger<RabitMqSender> _logger;
        private readonly ILogger _customLogger;

        public RabitMqSender(ILogger<RabitMqSender> logger, ILoggerFactory loggerFactory, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _logger = logger;
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), $"logger{string.Format("{0:yyyy-MM-dd}", DateTime.Now)}.txt"));
            _customLogger = loggerFactory.CreateLogger("FileLogger");

            _rabbitMqConfiguration = rabbitMqOptions.Value;

            CreateConnection();
        }

        public void SendMessage(object message)
        {
            if (ConnectionExists() && _rabbitMqConfiguration.Enabled)
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _rabbitMqConfiguration.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: _rabbitMqConfiguration.QueueName, basicProperties: null, body: body);

                    _logger.LogInformation(json, "RebitMq send message");
                    _customLogger.LogInformation(json, "RebitMq send message");   
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

                _logger.LogDebug("Connection created");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not create connection");
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
