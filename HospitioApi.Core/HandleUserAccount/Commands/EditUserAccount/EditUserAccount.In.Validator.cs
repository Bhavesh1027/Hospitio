

using FluentValidation;

namespace HospitioApi.Core.HandleUserAccount.Commands.EditUserAccount;

public class EditUserAccountValidator : AbstractValidator<EditUserAccountRequest>
{
    public EditUserAccountValidator()
    {
        RuleFor(m => m.In).SetValidator(new EditUserAccountInValidator());
    }

    public class EditUserAccountInValidator : AbstractValidator<EditUserAccountIn>
    {
        public EditUserAccountInValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty();
            RuleFor(m => m.LastName).NotEmpty();
            RuleFor(m => m.Email).NotEmpty();

        }
    }
}