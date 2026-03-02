using FluentValidation;
using HospitioApi.Core.HandleUserAccount.Commands.CreateEditUserAccount;
using static HospitioApi.Core.HandleUserAccount.Commands.CreateEditUserAccount.CreateUserAccountValidator;

namespace HospitioApi.Core.HandleCustomerUsers.Commands.CreateCustomerUser;

public class CreateCustomerUserValidator : AbstractValidator<CreateCustomerUserRequest>
{
    public CreateCustomerUserValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerUserInValidator());
    }
    public class CreateCustomerUserInValidator : AbstractValidator<CreateCustomerUserIn>
    {
        public CreateCustomerUserInValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty();
            RuleFor(m => m.LastName).NotEmpty();
            RuleFor(m => m.Email).NotEmpty();
        }
    }
}
