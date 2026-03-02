using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTicket.Commands.CloseTicket;

public class CloseTicketOut : BaseResponseOut
{
    public CloseTicketOut(string message) : base(message) { }
}
