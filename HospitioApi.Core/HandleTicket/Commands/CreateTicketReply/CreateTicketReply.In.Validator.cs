using FluentValidation;


namespace HospitioApi.Core.HandleTicket.Commands.CreateTicketReply;

public class CreateTicketReplyValidator : AbstractValidator<CreateTicketReplyRequest>
{
    public CreateTicketReplyValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateTicketReplyInValidator());
    }
    public class CreateTicketReplyInValidator : AbstractValidator<CreateTicketReplyIn>
    {
        public CreateTicketReplyInValidator()
        {
            RuleFor(m => m.TicketId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.Reply).NotEmpty();
        }
    }
}