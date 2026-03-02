using FluentValidation;

namespace HospitioApi.Core.HandleAccount.Commands.ResetPassword;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordHandlerRequest>
{
    public ResetPasswordValidator()
    {
        RuleFor(m => m.In).SetValidator(new ResetPasswordInValidator());
    }

    public class ResetPasswordInValidator : AbstractValidator<ResetPasswordIn>
    {
        public ResetPasswordInValidator()
        {
            RuleFor(m => m.Password).NotEmpty().NotNull();
            RuleFor(m => m.Token).NotEmpty().NotNull();
        }
    }
}
