using FluentValidation;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.UpdateIsActiveCustomersDigitalAssistants;

public class UpdateIsActiveCustomersDigitalAssistantsValidator : AbstractValidator<UpdateIsActiveCustomersDigitalAssistantsRequest>
{
    public UpdateIsActiveCustomersDigitalAssistantsValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateIsActiveCustomersDigitalAssistantsInValidator());
    }

    public class UpdateIsActiveCustomersDigitalAssistantsInValidator : AbstractValidator<UpdateIsActiveCustomersDigitalAssistantsIn>
    {
        public UpdateIsActiveCustomersDigitalAssistantsInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.IsActive).NotEmpty();
        }
    }
}
