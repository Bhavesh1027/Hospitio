using FluentValidation;

namespace HospitioApi.Core.HandleGroups.Queries.GetGroup;

public class GetGroupValidator : AbstractValidator<GetGroupHandlerRequest>
{
    public GetGroupValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetGroupInValidator());
    }

    public class GetGroupInValidator : AbstractValidator<GetGroupIn>
    {
        public GetGroupInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
