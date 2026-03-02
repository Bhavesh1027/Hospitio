using FluentValidation;

namespace HospitioApi.Core.HandleTicket.Queries.GetTickets;
public class GetTicketsValidator : AbstractValidator<GetTicketsRequest>
{
    public GetTicketsValidator()
    {
    }

    public class GetTicketsInValidator : AbstractValidator<GetTicketsIn>
    {
        public GetTicketsInValidator()
        {
        }
    }
}
