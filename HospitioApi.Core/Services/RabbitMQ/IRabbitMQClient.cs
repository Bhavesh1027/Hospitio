using HospitioApi.Core.HandleVonage.Commands.InboundWebhook;
using HospitioApi.Core.Services.BackGroundServiceData;

namespace HospitioApi.Core.RabbitMQ;

public interface IRabbitMQClient
{ 
    Task SendCustomer(CustomerAction customerAction);
    Task SendGuestMessage(CustomerGuestJorneyDetails guestMessage);
    Task GuestMessage(int CustomerId,int CustomerUserId);
    Task ReceiveWPMessage(GetInboundWebhookIn getInboundWebhookIn);
}
