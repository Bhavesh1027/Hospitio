using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using HospitioApi.Core.Options;
using HospitioApi.Core.RabbitMQ;
using HospitioApi.Core.Services.BackGroundServiceData;
using System.Text;

namespace HospitioApi.BackGroundService.Receiver;

public class CustomerQueueConsume : BackgroundService
{
    private readonly ILogger<CustomerQueueConsume> _logger;
    private IRabbitMQClient _rabbit;
    private readonly RabbitMQSettingsOptions _rabbitMQ;
    private readonly BackGroundServicesSettingsOptions _time;
    public CustomerQueueConsume(ILogger<CustomerQueueConsume> logger, IRabbitMQClient rabbit, IOptions<RabbitMQSettingsOptions> rabbiMQ, IOptions<BackGroundServicesSettingsOptions> time)
    {
        _rabbitMQ = rabbiMQ.Value;
        _rabbit = rabbit;
        _logger = logger;
        _time = time.Value;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer Queue Comsume Service Started");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service Stepped");
        return Task.CompletedTask;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQ.HostName,
                Port = _rabbitMQ.Port,
                UserName = _rabbitMQ.UserName,
                Password = _rabbitMQ.Password
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(_rabbitMQ.Exchange, ExchangeType.Direct);
            channel.QueueDeclare(_rabbitMQ.CustomerQueue,
                true, false, false, null);

            channel.QueueBind(_rabbitMQ.CustomerQueue, _rabbitMQ.Exchange,
                "hospitio.customer");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<CustomerAction>(json);
                switch (message.ActionName)
                {
                    case "CUSTOMER_MAIN":
                        await _rabbit.GuestMessage(message.CustomerId,message.CustomerUserId);
                        break;
                }
                //channel.BasicAck(ea.DeliveryTag, multiple: false);
            };
            channel.BasicConsume(queue: _rabbitMQ.CustomerQueue,
                    autoAck: true,
                    consumer: consumer);

            _logger.LogInformation($"Customer Queue Comsume Service Running{DateTime.Now}");
            await Task.Delay(TimeSpan.Parse(_time.CustomerQueueConsumeTiming), stoppingToken);
        }
    }

}
