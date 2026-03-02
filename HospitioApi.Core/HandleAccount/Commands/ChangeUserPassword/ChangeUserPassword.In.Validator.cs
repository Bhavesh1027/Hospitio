using FluentValidation;

namespace HospitioApi.Core.HandleAccount.Commands.ChangeUserPassword;
public class ChangeUserPasswordHandlerRequestValidator : AbstractValidator<ChangeUserPasswordHandlerRequest>
{
    public ChangeUserPasswordHandlerRequestValidator()
    {
        RuleFor(m => m.In).SetValidator(new ChangeUserPasswordInValidator());
        RuleFor(m => m.UserId).GreaterThan(0);
    }

    public class ChangeUserPasswordInValidator : AbstractValidator<ChangeUserPasswordIn>
    {
        public ChangeUserPasswordInValidator()
        {
            RuleFor(m => m.NewPassword).NotNull().NotEmpty();
            RuleFor(m => m.OldPassword).NotNull().NotEmpty();
            RuleFor(m => m.NewPassword).NotEqual(m => m.OldPassword).WithMessage("New Password Should Not Same as Old Password");
        }
    }
}