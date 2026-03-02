using FluentValidation;

namespace HospitioApi.Core.HandleAccount.Commands.RefreshToken;
public class RefreshTokenHandlerRequestValidator : AbstractValidator<RefreshTokenHandlerRequest>
{
    public RefreshTokenHandlerRequestValidator()
    {
        RuleFor(m => m.In).SetValidator(new RefreshTokenInValidator());
    }

    public class RefreshTokenInValidator : AbstractValidator<RefreshTokenIn>
    {
        public RefreshTokenInValidator()
        {
            RuleFor(m => m.TokenValue)
                .NotEmpty();

            RuleFor(m => m.TokenId)
                .NotEmpty();
        }
    }
}
