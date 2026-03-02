using FluentValidation;
namespace HospitioApi.Core.HandleDepartment.Commands.EditDepartment;
public class EditDepartmentValidator : AbstractValidator<EditDepartmentRequest>
{
    public EditDepartmentValidator()
    {
        RuleFor(m => m.In).SetValidator(new EditDepartmentInValidator());
        RuleFor(m => m.Id).NotNull().GreaterThan(0);
    }

    public class EditDepartmentInValidator : AbstractValidator<EditDepartmentIn>
    {
        public EditDepartmentInValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}
