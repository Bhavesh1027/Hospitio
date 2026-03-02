using FluentValidation;


namespace HospitioApi.Core.HandleTicket.Commands.CreateTicket;

public class CreateTicketValidator : AbstractValidator<CreateTicketRequest>
{
    public CreateTicketValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateTicketInValidator());
    }
    public class CreateTicketInValidator : AbstractValidator<CreateTicketIn>
    {
        public CreateTicketInValidator()
        {
            //RuleFor(m => m.CSAgentId).NotEmpty().GreaterThan(0);
            //RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
            //RuleFor(m => m.TicketCategoryId).NotEmpty().GreaterThan(0);
        }
    }
}