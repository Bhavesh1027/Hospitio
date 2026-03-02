using FluentValidation;

namespace HospitioApi.Core.HandleGroups.Commands.DeleteGroup;

public class DeleteGroupValidator : AbstractValidator<DeleteGroupRequest>
{
    public DeleteGroupValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteGroupInValidator());
    }
    public class DeleteGroupInValidator : AbstractValidator<DeleteGroupIn>
    {
        public DeleteGroupInValidator()
        {
            RuleFor(m => m.GroupId).NotEmpty().GreaterThan(0);
        }
    }
}

