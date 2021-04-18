using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqPostman.BLL.Models;

namespace RabbitMqPostman.BLL.RebitMq
{
    public abstract class RabbitMqListener<T> : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly RabbitMqConfiguration _rabbitMqConfiguration;

        public RabbitMqListener(RabbitMqConfiguration rabbitMqOptions)
        {
            _rabbitMqConfiguration = rabbitMqOptions;

            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqConfiguration.Hostname,
                UserName = _rabbitMqConfiguration.UserName,
                Password = _rabbitMqConfiguration.Password
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _rabbitMqConfiguration.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonConvert.DeserializeObject<T>(content);

                HandleMessage(message);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            _channel.BasicConsume(_rabbitMqConfiguration.QueueName, false, consumer);

            return Task.CompletedTask;
        }

        public abstract void HandleMessage(T message);

        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
        { }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        { }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        { }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        { }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        { }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
