using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using cyberplace_product_search_ms.Controllers.Models;
using cyberplace_product_search_ms.Models;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace cyberplace_product_search_ms.Services
{
    public class RabbitReceiverService : BackgroundService
    {
        const string QueueName = "shistory_history_q";
        private IServiceProvider _sp;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public SearchHistoryService _searchHistoryService;
        public RabbitReceiverService(IRabbitMQSettings settings, IServiceProvider sp, SearchHistoryService searchHistoryService)
        {
            _sp = sp;
            _factory = new ConnectionFactory() { HostName = settings.Host, Port = settings.Port };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _searchHistoryService = searchHistoryService;

            _channel.QueueDeclare(  queue: QueueName,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                _channel.Dispose();
                _connection.Dispose();
                return Task.CompletedTask;
            }

            var channel1 = _channel;

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                //Console.WriteLine("Mensaje recibido: {0} ", message);
                Task.Run(() =>
                {
                    UserQueue user = Newtonsoft.Json.JsonConvert.DeserializeObject<UserQueue>(message);
                    string username = user.Username;
                    SearchHistory searchHistory = new SearchHistory();
                    searchHistory.Username = username;
                    _searchHistoryService.Create(searchHistory);

                });
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };


            _channel.BasicConsume(queue: QueueName,
                                 autoAck: false,
                                 consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
