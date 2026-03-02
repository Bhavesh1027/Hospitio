using FluentValidation;

namespace HospitioApi.Core.HandleAccount.Commands.ResetPasswordConfirmation;

public class ResetPasswordConfirmationValidator : AbstractValidator<ResetPasswordConfirmationHandlerRequest>
{
    public ResetPasswordConfirmationValidator()
    {
        RuleFor(m => m.In).SetValidator(new ResetPasswordConfirmationInValidator());
    }

    public class ResetPasswordConfirmationInValidator : AbstractValidator<ResetPasswordConfirmationIn>
    {
        public ResetPasswordConfirmationInValidator()
        {
            RuleFor(m => m.Email).NotNull().NotEmpty();
        }
    }
}
