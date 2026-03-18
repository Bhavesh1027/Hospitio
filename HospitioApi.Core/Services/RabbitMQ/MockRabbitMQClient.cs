using HospitioApi.Core.HandleVonage.Commands.InboundWebhook;
using HospitioApi.Core.Services.BackGroundServiceData;

namespace HospitioApi.Core.RabbitMQ;

public class MockRabbitMQClient : IRabbitMQClient
{ 
    public Task SendCustomer(CustomerAction customerAction)
    {
        return Task.CompletedTask;
    }

    public Task SendGuestMessage(CustomerGuestJorneyDetails guestMessage)
    {
        return Task.CompletedTask;
    }

    public Task GuestMessage(int CustomerId, int CustomerUserId)
    {
        return Task.CompletedTask;
    }

    public Task ReceiveWPMessage(GetInboundWebhookIn getInboundWebhookIn)
    {
        return Task.CompletedTask;
    }
}
