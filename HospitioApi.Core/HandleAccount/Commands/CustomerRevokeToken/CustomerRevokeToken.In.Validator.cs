using FluentValidation;

namespace HospitioApi.Core.HandleAccount.Commands.CustomerRevokeToken;

public class CustomerRevokeTokenValidator : AbstractValidator<CustomerRevokeTokenHandlerRequest>
{
    public CustomerRevokeTokenValidator()
    {
        RuleFor(m => m.In).SetValidator(new CustomerRevokeTokenInValidator());
    }

    public class CustomerRevokeTokenInValidator : AbstractValidator<CustomerRevokeTokenIn>
    {
        public CustomerRevokeTokenInValidator()
        {
            RuleFor(m => m.TokenValue)
                .NotEmpty();

            RuleFor(m => m.TokenId)
                .NotEmpty();
        }
    }
}
