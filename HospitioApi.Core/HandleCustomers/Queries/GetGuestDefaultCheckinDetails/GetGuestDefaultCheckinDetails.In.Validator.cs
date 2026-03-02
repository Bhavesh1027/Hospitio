using FluentValidation;

namespace HospitioApi.Core.HandleCustomers.Queries.GetGuestDefaultCheckinDetails;

public class GetGuestDefaultCheckinDetailsValidator : AbstractValidator<GetGuestDefaultCheckinDetailsRequest>
{
    public GetGuestDefaultCheckinDetailsValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetGuestDefaultCheckinDetailsInValidator());
    }
    public class GetGuestDefaultCheckinDetailsInValidator : AbstractValidator<GetGuestDefaultCheckinDetailsIn>
    {
        public GetGuestDefaultCheckinDetailsInValidator()
        {
            RuleFor(m => m.CustomerId).NotEmpty().NotNull();
        }
    }
}
