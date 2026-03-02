using FluentValidation;

namespace HospitioApi.Core.HandleAccount.Commands.CustomerRefreshToken;

public class CustomerRefreshTokenValidator : AbstractValidator<CustomerRefreshTokenHandlerRequest>
{
    public CustomerRefreshTokenValidator()
    {
        RuleFor(m => m.In).SetValidator(new CustomerRefreshTokenInValidator());
    }

    public class CustomerRefreshTokenInValidator : AbstractValidator<CustomerRefreshTokenIn>
    {
        public CustomerRefreshTokenInValidator()
        {
            RuleFor(m => m.TokenValue)
                .NotEmpty();

            RuleFor(m => m.TokenId)
                .NotEmpty();
        }
    }
}
