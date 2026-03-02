using FluentValidation;

namespace HospitioApi.Core.HandleTicket.Commands.UpdateTicketPriority;

public class UpdateTicketPriorityValidator : AbstractValidator<UpdateTicketPriorityRequest>
{
    public UpdateTicketPriorityValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateTicketPriorityInValidator());
    }

    public class UpdateTicketPriorityInValidator : AbstractValidator<UpdateTicketPriorityIn>
    {
        public UpdateTicketPriorityInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
