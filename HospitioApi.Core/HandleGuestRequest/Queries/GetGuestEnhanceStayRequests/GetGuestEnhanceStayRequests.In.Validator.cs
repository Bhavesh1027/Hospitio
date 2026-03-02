using FluentValidation;

namespace HospitioApi.Core.HandleGuestRequest.Queries.GetGuestEnhanceStayRequests;

public class GetGuestEnhanceStayRequestsValidator: AbstractValidator<GetGuestEnhanceStayRequestsRequest>
{
    public GetGuestEnhanceStayRequestsValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetGuestRequestsInValidator());
    }
    public class GetGuestRequestsInValidator : AbstractValidator<GetGuestEnhanceStayRequestsIn>
    {
        public GetGuestRequestsInValidator()
        {
            RuleFor(m => m.CustomerId).NotNull().GreaterThan(0);
            RuleFor(m => m.GuestId).NotNull().GreaterThan(0);
        }
    }
}
