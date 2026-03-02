using FluentValidation;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Queries.GetCustomersGuestJourneysById;

public class GetCustomersGuestJourneysByIdValidator : AbstractValidator<GetCustomersGuestJourneysByIdRequest>
{
    public GetCustomersGuestJourneysByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomersGuestJourneysByIdInValidator());
    }
    public class GetCustomersGuestJourneysByIdInValidator : AbstractValidator<GetCustomersGuestJourneysByIdIn>
    {
        public GetCustomersGuestJourneysByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
