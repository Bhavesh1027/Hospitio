using FluentValidation;

namespace HospitioApi.Core.HandleAccount.Commands.Login;
public class LoginHandlerRequestValidator : AbstractValidator<LoginHandlerRequest>
{
    public LoginHandlerRequestValidator()
    {
        RuleFor(m => m.In).SetValidator(new LoginInValidator());
    }

    public class LoginInValidator : AbstractValidator<LoginIn>
    {
        public LoginInValidator()
        {
            RuleFor(m => m.Email)
                .NotEmpty();

            RuleFor(m => m.Password)
                .NotEmpty();
        }
    }
}