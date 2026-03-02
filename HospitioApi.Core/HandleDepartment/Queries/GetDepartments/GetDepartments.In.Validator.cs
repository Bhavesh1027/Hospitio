using FluentValidation;
namespace HospitioApi.Core.HandleDepartment.Queries.GetDepartments;
public class GetDepartmentsValidator : AbstractValidator<GetDepartmentsRequest>
{
    public GetDepartmentsValidator()
    {
    }

    public class GetDepartmentsInValidator : AbstractValidator<GetDepartmentsIn>
    {
        public GetDepartmentsInValidator()
        {
        }
    }
}
