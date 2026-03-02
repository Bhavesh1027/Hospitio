using FluentValidation;

namespace HospitioApi.Core.HandleAccount.Commands.RevokeToken;

public class RevokeTokenHandlerRequestValidator : AbstractValidator<RevokeTokenHandlerRequest>
{
    public RevokeTokenHandlerRequestValidator()
    {
        RuleFor(m => m.In).SetValidator(new RevokeTokenInValidator());
    }

    public class RevokeTokenInValidator : AbstractValidator<RevokeTokenIn>
    {
        public RevokeTokenInValidator()
        {
            RuleFor(m => m.TokenValue)
                .NotEmpty();

            RuleFor(m => m.TokenId)
                .NotEmpty();
        }
    }
}

