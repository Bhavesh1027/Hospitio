using FluentValidation;

namespace HospitioApi.Core.HandleGroups.Queries.GetGroupsByDepartmentId;

public class GetGroupsByDepartmentId: AbstractValidator<GetGroupsByDepartmentIdRequest>
{
    public GetGroupsByDepartmentId()
    {
        RuleFor(m => m.In).SetValidator(new GetGroupByDepartmentIdInValidator());
    }

    public class GetGroupByDepartmentIdInValidator : AbstractValidator<GetGroupsByDepartmentIdIn>
    {
        public GetGroupByDepartmentIdInValidator()
        {
            RuleFor(m => m.DepartmentId).NotEmpty().GreaterThan(0);
        }
    }
}
