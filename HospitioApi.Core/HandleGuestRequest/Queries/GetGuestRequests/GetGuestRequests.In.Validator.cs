using FluentValidation;

namespace HospitioApi.Core.HandleGuestRequest.Queries.GetGuestRequests;

public class GetGuestRequestsValidator : AbstractValidator<GetGuestRequestsRequest>
{
    public GetGuestRequestsValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetGuestRequestsInValidator());
        RuleFor(m => m.CustomerId).GreaterThan(0);
    }
    public class GetGuestRequestsInValidator : AbstractValidator<GetGuestRequestsIn>
    {
        public GetGuestRequestsInValidator()
        {
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
        }
    }
}
