using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTicket.Commands.UpdateTicketPriority;

public class UpdateTicketPriorityOut : BaseResponseOut
{
    public UpdateTicketPriorityOut(string message, UpdatedTicketPriorityOut updatedTicketStatusOut) : base(message)
    {
        UpdatedTicketStatusOut = updatedTicketStatusOut;
    }
    public UpdatedTicketPriorityOut UpdatedTicketStatusOut { get; set; }
}
public class UpdatedTicketPriorityOut
{
    public int Id { get; set; }
    public byte? Priority { get; set; }
}
