namespace HospitioApi.Core.HandleTicket.Commands.UpdateTicketPriority;

public class UpdateTicketPriorityIn
{
    public int Id { get; set; }
    public byte? Priority { get; set; }
}
