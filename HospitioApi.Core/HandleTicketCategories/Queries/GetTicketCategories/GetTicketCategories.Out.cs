using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTicketCategories.Queries.GetTicketCategories;

public class GetTicketCategoriesOut : BaseResponseOut
{
    public GetTicketCategoriesOut(string message, List<TicketCategoriesOut> ticketCategoriesOuts) : base(message)
    {
        TicketCategoriesOuts = ticketCategoriesOuts;
    }
    public List<TicketCategoriesOut> TicketCategoriesOuts { get; set; } = new List<TicketCategoriesOut>();
}
public class TicketCategoriesOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
