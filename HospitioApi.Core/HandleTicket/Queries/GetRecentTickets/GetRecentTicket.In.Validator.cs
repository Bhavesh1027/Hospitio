using FluentValidation;

namespace HospitioApi.Core.HandleTicket.Queries.GetRecentTickets;

public class GetRecentTicketValidator : AbstractValidator<GetRecentTicketsRequest>
{
    public GetRecentTicketValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetRecentTicketsInValidator());

    }
    public class GetRecentTicketsInValidator : AbstractValidator<GetRecentTicketIn>
    {
        public GetRecentTicketsInValidator()
        {
            //RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
        }
    }
}
