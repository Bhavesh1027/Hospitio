using FluentValidation;
using HospitioApi.Core.HandleDepartment.Queries.GetDepartmentById;

namespace HospitioApi.Core.HandleDepartment.Queries.GetCustomerDepartmentById.cs;

public class GetCustomerDepartmentByIdValidator : AbstractValidator<GetDepartmentByIdRequest>
{
    public GetCustomerDepartmentByIdValidator()
    {
        RuleFor(m => m.Id).GreaterThan(0);
    }
}
