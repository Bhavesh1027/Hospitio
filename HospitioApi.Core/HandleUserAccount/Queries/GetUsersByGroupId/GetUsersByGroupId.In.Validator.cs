using FluentValidation;

namespace HospitioApi.Core.HandleUserAccount.Queries.GetUsersByGroupId;

public class GetUsersByGroupIdValidator: AbstractValidator<GetUsersByGroupIdRequest>
{
    public GetUsersByGroupIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetUserByIdValidatorInValidator());
    }

    public class GetUserByIdValidatorInValidator : AbstractValidator<GetUsersByGroupIdIn>
    {
        public GetUserByIdValidatorInValidator()
        {
            RuleFor(m => m.GroupId).NotEmpty().GreaterThan(0);
        }
    }
}
