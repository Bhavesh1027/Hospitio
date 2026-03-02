using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTicket.Commands.CreateTicket;


public class CreateTicketOut : BaseResponseOut
{
    public CreateTicketOut(string message) : base(message)
    {
    }
}
