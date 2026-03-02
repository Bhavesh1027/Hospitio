using FluentValidation;
namespace HospitioApi.Core.HandleDepartment.Commands.CreateDepartment;
public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateDepartmentInValidator());
    }

    public class CreateDepartmentInValidator : AbstractValidator<CreateDepartmentIn>
    {
        public CreateDepartmentInValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}
