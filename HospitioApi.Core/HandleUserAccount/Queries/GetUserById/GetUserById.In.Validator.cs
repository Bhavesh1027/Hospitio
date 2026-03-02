using FluentValidation;

namespace HospitioApi.Core.HandleUserAccount.Queries.GetUserById;

public class GetUserByIdValidator : AbstractValidator<GetUserByIdRequest>
{
    public GetUserByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetUserByIdValidatorInValidator());
    }

    public class GetUserByIdValidatorInValidator : AbstractValidator<GetUserByIdIn>
    {
        public GetUserByIdValidatorInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}