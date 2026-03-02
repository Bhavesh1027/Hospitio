using FluentValidation;

namespace HospitioApi.Core.HandleGroups.Commands.CreateGroup;

public class CreateGroupValidator : AbstractValidator<CreateGroupRequest>
{
    public CreateGroupValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateGroupInValidator());
    }
    public class CreateGroupInValidator : AbstractValidator<CreateGroupIn>
    {
        public CreateGroupInValidator()
        {
            RuleFor(m => m.DepartmentId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}
