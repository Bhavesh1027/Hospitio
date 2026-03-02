using FluentValidation;

namespace HospitioApi.Core.HandleCustomerUsers.Commands.EditCustomerUser;

public class EditCustomerUserValidator : AbstractValidator<EditCustomerUserRequest>
{
    public EditCustomerUserValidator()
    {
        RuleFor(m => m.In).SetValidator(new EditCustomerUserAccountInValidator());
    }

    public class EditCustomerUserAccountInValidator : AbstractValidator<EditCustomerUserIn>
    {
        public EditCustomerUserAccountInValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty();
            RuleFor(m => m.LastName).NotEmpty();
            RuleFor(m => m.Email).NotEmpty();

        }
    }
}
