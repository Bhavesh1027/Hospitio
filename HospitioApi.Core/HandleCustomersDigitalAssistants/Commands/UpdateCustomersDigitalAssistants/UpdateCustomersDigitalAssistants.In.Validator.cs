using FluentValidation;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.UpdateCustomersDigitalAssistants;

public class UpdateCustomersDigitalAssistantsValidator : AbstractValidator<UpdateCustomersDigitalAssistantsRequest>
{
    public UpdateCustomersDigitalAssistantsValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomersDigitalAssistantsInValidator());
    }

    public class UpdateCustomersDigitalAssistantsInValidator : AbstractValidator<UpdateCustomersDigitalAssistantsIn>
    {
        public UpdateCustomersDigitalAssistantsInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.Details).NotEmpty();
            RuleFor(m => m.Icon).NotEmpty();
        }
    }
}
