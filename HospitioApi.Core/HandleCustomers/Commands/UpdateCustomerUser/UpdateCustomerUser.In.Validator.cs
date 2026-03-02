using FluentValidation;

namespace HospitioApi.Core.HandleCustomers.Commands.UpdateCustomerUser;

public class UpdateCustomerUserValidator : AbstractValidator<UpdateCustomerUserRequest>
{
    public UpdateCustomerUserValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerUserInValidation());
    }

    public class UpdateCustomerUserInValidation : AbstractValidator<UpdateCustomerUserIn>
    {
        public UpdateCustomerUserInValidation()
        {
            RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.UserName).NotEmpty();
            RuleFor(m => m.PhoneCountry).NotEmpty().MaximumLength(3);
        }
    }
}
