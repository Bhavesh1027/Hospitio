using FluentValidation;

namespace HospitioApi.Core.HandleTicket.Commands.ForwardTicket;

public class ForwardTicketValidator : AbstractValidator<ForwardTicketRequest>
{
    public ForwardTicketValidator()
    {
        RuleFor(m => m.In).SetValidator(new ForwardTicketInValidator());
    }

    public class ForwardTicketInValidator : AbstractValidator<ForwardTicketIn>
    {
        public ForwardTicketInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            //RuleFor(m => m.UserId).NotEmpty().GreaterThan(0);
        }
    }
}
