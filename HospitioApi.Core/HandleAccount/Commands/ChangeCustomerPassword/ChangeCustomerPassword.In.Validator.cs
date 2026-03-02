using FluentValidation;

namespace HospitioApi.Core.HandleAccount.Commands.ChangeCustomerPassword;
public class ChangeCustomerPasswordHandlerRequestValidator : AbstractValidator<ChangeCustomerPasswordHandlerRequest>
{
    public ChangeCustomerPasswordHandlerRequestValidator()
    {
        RuleFor(m => m.In).SetValidator(new ChangeCustomerPasswordInValidator());
        RuleFor(m => m.customerId).GreaterThan(0);
    }

    public class ChangeCustomerPasswordInValidator : AbstractValidator<ChangeCustomerPasswordIn>
    {
        public ChangeCustomerPasswordInValidator()
        {
            RuleFor(m => m.NewPassword).NotNull().NotEmpty();
            RuleFor(m => m.OldPassword).NotNull().NotEmpty();
            RuleFor(m => m.NewPassword).NotEqual(m => m.OldPassword).WithMessage("New Password Should Not Same as Old Password");
        }
    }
}