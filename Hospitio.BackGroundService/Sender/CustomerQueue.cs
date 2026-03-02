using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Core.RabbitMQ;
using HospitioApi.Core.Services.BackGroundServiceData;

namespace HospitioApi.BackGroundService.Sender;

public class CustomerQueue : BackgroundService
{
    private IRabbitMQClient _rabbit;
    private readonly ILogger<CustomerQueue> _logger;
    private readonly IBackGroundServiceData _backGroundServiceData;
    private readonly BackGroundServicesSettingsOptions _time;
    public CustomerQueue(ILogger<CustomerQueue> logger, IRabbitMQClient rabbit, IBackGroundServiceData backGroundServiceData, IOptions<BackGroundServicesSettingsOptions> time)
    {
        _logger = logger;
        _rabbit = rabbit;
        _backGroundServiceData = backGroundServiceData;
        _time = time.Value;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "{Type} is now running in the background",
            nameof(CustomerQueue));

        await BackgroundProcessing(stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogCritical(
            "The {Type} is stopping due to a host shutdown, queued items might not be processed anymore.",
            nameof(CustomerQueue));

        return base.StopAsync(cancellationToken);
    }
    protected async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var customerIds = await _backGroundServiceData.GetCustomers(stoppingToken);

                foreach (var customerId in customerIds)
                {
                    CustomerAction customerAction = new CustomerAction();
                    customerAction.CustomerId = Convert.ToInt32(customerId.Id);
                    customerAction.CustomerUserId = Convert.ToInt32(customerId.CustomerUserId);
                    customerAction.ActionName = "CUSTOMER_MAIN";
                    _rabbit.SendCustomer(customerAction);
                }
            }

            catch (Exception e)
            {
                _logger.LogError(e, "Error during Get Data. testtest");
            }

            _logger.LogInformation($"Customer Queue Service Running{DateTime.Now}");

            await Task.Delay(TimeSpan.Parse(_time.CustomerQueueTiming), stoppingToken);
        }
    }
}
