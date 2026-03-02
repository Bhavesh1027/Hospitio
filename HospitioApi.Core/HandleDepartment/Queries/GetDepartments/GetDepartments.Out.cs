using HospitioApi.Shared;

namespace HospitioApi.Core.HandleDepartment.Queries.GetDepartments;

public class GetDepartmentsOut : BaseResponseOut
{
    public GetDepartmentsOut(string message, List<GetDepartmentsResponseOut> GetDepartmentsResponseOut) : base(message)
    {
        getDepartmentsResponseOut = GetDepartmentsResponseOut;
    }

    public List<GetDepartmentsResponseOut> getDepartmentsResponseOut { get; set; } = new List<GetDepartmentsResponseOut>();

}

public class GetDepartmentsResponseOut
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? DepartmentMangerId { get; set; }
}

