using FluentValidation;


namespace HospitioApi.Core.HandleUserAccount.Commands.CreateEditUserAccount;

public class CreateUserAccountValidator : AbstractValidator<CreateUserAccountRequest>
{
    public CreateUserAccountValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateUserAccountInValidator());
    }

    public class CreateUserAccountInValidator : AbstractValidator<CreateUserAccountIn>
    {
        public CreateUserAccountInValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty();
            RuleFor(m => m.LastName).NotEmpty();
            RuleFor(m => m.Email).NotEmpty();
        }
    }
}