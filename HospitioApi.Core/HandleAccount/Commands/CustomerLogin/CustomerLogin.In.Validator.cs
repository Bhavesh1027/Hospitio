using FluentValidation;

namespace HospitioApi.Core.HandleAccount.Commands.CustomerLogin;
public class CustomerLoginHandlerRequestValidator : AbstractValidator<CustomerLoginHandlerRequest>
{
    public CustomerLoginHandlerRequestValidator()
    {
        RuleFor(m => m.In).SetValidator(new CustomerLoginInValidator());
    }

    public class CustomerLoginInValidator : AbstractValidator<CustomerLoginIn>
    {
        public CustomerLoginInValidator()
        {
            RuleFor(m => m.Email)
                .NotEmpty();

            RuleFor(m => m.Password)
                .NotEmpty();
        }
    }
}