using FluentValidation;

namespace HospitioApi.Core.HandleUserAccount.Commands.UpdateUserStatus;

public class UpdateUserStatusValidator : AbstractValidator<UpdateUserStatusRequest>
{
    public UpdateUserStatusValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateUserStatusInValidator());
    }

    public class UpdateUserStatusInValidator : AbstractValidator<UpdateUserStatusIn>
    {
        public UpdateUserStatusInValidator()
        {
            RuleFor(m => m.UserId).GreaterThan(0);
        }
    }
}
