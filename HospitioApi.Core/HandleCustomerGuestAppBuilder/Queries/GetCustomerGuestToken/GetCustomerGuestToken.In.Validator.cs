using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestToken;

public class GetCustomerGuestTokenValidator : AbstractValidator<GetCustomerGuestTokenRequest>
{
    public GetCustomerGuestTokenValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerGuestTokenInValidator());
    }

    public class GetCustomerGuestTokenInValidator : AbstractValidator<GetCustomerGuestTokenIn>
    {
        public GetCustomerGuestTokenInValidator()
        {
            RuleFor(m => m.Id).NotEmpty();
        }
    }
}
