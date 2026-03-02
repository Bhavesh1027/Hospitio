using FluentValidation;
namespace HospitioApi.Core.HandleDepartment.Queries.GetDepartmentById;
public class GetDepartmentByIdValidator : AbstractValidator<GetDepartmentByIdRequest>
{
    public GetDepartmentByIdValidator()
    {
        RuleFor(m => m.Id).GreaterThan(0);
    }
}
