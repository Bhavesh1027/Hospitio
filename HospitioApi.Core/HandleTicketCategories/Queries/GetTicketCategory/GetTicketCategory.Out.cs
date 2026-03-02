using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTicketCategories.Queries.GetTicketCategory;

public class GetTicketCategoryOut : BaseResponseOut
{
    public GetTicketCategoryOut(string message, TicketCategoryOut ticketCategoryOut) : base(message)
    {
        TicketCategoryOut = ticketCategoryOut;
    }
    public TicketCategoryOut TicketCategoryOut { get; set; }
}
public class TicketCategoryOut
{
    public int Id { get; set; }
    public string Name { get; set; } = null;
}
