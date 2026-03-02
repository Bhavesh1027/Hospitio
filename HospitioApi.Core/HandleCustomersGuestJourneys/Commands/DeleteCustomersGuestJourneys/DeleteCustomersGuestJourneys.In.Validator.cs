using FluentValidation;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Commands.DeleteCustomersGuestJourneys;

public class DeleteCustomersGuestJourneysValidator : AbstractValidator<DeleteCustomersGuestJourneysRequest>
{
    public DeleteCustomersGuestJourneysValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomersGuestJourneysInValidator());
    }
    public class DeleteCustomersGuestJourneysInValidator : AbstractValidator<DeleteCustomersGuestJourneysIn>
    {
        public DeleteCustomersGuestJourneysInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
