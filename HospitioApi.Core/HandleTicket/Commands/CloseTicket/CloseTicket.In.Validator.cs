using FluentValidation;

namespace HospitioApi.Core.HandleTicket.Commands.CloseTicket;

public class CloseTicketValidator : AbstractValidator<CloseTicketRequest>
{
    public CloseTicketValidator()
    {
        RuleFor(m => m.In).SetValidator(new CloseTicketInValidator());
    }
    public class CloseTicketInValidator : AbstractValidator<CloseTicketIn>
    {
        public CloseTicketInValidator()
        {
            RuleFor(m => m.TicketId).NotEmpty().GreaterThan(0);
        }
    }
}
