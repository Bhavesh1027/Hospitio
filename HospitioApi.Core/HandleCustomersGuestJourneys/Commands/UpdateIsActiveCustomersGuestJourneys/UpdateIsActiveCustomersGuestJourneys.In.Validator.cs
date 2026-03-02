using FluentValidation;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Commands.UpdateIsActiveCustomersGuestJourneys;

public class UpdateIsActiveCustomersGuestJourneysValidator : AbstractValidator<UpdateIsActiveCustomersGuestJourneysRequest>
{
    public UpdateIsActiveCustomersGuestJourneysValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateIsActiveCustomersGuestJourneysInValidator());
    }

    public class UpdateIsActiveCustomersGuestJourneysInValidator : AbstractValidator<UpdateIsActiveCustomersGuestJourneyIn>
    {
        public UpdateIsActiveCustomersGuestJourneysInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.IsActive).NotEmpty();
        }
    }
}
