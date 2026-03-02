using FluentValidation;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.DeleteCustomersDigitalAssistants;

public class DeleteCustomersDigitalAssistantsValidator : AbstractValidator<DeleteCustomersDigitalAssistantsRequest>
{
    public DeleteCustomersDigitalAssistantsValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomersDigitalAssistantsInValidator());
    }
    public class DeleteCustomersDigitalAssistantsInValidator : AbstractValidator<DeleteCustomersDigitalAssistantsIn>
    {
        public DeleteCustomersDigitalAssistantsInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
