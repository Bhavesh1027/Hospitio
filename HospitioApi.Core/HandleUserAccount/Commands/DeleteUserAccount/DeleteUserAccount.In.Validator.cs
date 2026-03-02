using FluentValidation;


namespace HospitioApi.Core.HandleUserAccount.Commands.DeleteUserAccount;

public class DeleteUserAccountValidator : AbstractValidator<DeleteUserAccountRequest>
{
    public DeleteUserAccountValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteUserAccountInValidator());
    }
    public class DeleteUserAccountInValidator : AbstractValidator<DeleteUserAccountIn>
    {
        public DeleteUserAccountInValidator()
        {
            RuleFor(m => m.UserId).NotEmpty().GreaterThan(0);
        }
    }
}
