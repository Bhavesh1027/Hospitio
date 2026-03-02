using FluentValidation;

namespace HospitioApi.Core.HandleGroups.Commands.UpdateGroup;

public class UpdateGroupValidator : AbstractValidator<UpdateGroupRequest>
{
    public UpdateGroupValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateGroupInValidator());
    }
    public class UpdateGroupInValidator : AbstractValidator<UpdateGroupIn>
    {
        public UpdateGroupInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.DepartmentId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.IsActive).NotEmpty();
        }
    }
}
