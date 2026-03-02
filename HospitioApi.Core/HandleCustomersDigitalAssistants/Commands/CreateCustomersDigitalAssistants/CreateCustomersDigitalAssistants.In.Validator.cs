using FluentValidation;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.CreateCustomersDigitalAssistants;

public class CreateCustomersDigitalAssistantsValidator : AbstractValidator<CreateCustomersDigitalAssistantsRequest>
{
    public CreateCustomersDigitalAssistantsValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomersDigitalAssistantsInValidator());
    }
    public class CreateCustomersDigitalAssistantsInValidator : AbstractValidator<CreateCustomersDigitalAssistantsIn>
    {
        public CreateCustomersDigitalAssistantsInValidator()
        {
            //RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.Details).NotEmpty();
            RuleFor(m => m.Icon).NotEmpty();
        }
    }
}
