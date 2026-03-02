using FluentValidation;
using HospitioApi.Core.HandleDepartment.Commands.CreateDepartment;

namespace HospitioApi.Core.HandleDepartment.Commands.CreateCustomerDepartment;

public class CreateCustomerDepartmentValidator : AbstractValidator<CreateCustomerDepartmentRequest>
{
    public CreateCustomerDepartmentValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerDepartmentInValidator());
    }

    public class CreateCustomerDepartmentInValidator : AbstractValidator<CreateCustomerDepartmentIn>
    {
        public CreateCustomerDepartmentInValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}
