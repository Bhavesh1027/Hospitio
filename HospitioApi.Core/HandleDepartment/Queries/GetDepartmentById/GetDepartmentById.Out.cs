using HospitioApi.Shared;

namespace HospitioApi.Core.HandleDepartment.Queries.GetDepartmentById;

public class GetDepartmentByIdOut : BaseResponseOut
{
    public GetDepartmentByIdOut(string message, GetDepartmentByIdResponseOut getDepartmentByIdResponseOut) : base(message)
    {
        GetDepartmentByIdResponseOut = getDepartmentByIdResponseOut;
    }


    public GetDepartmentByIdResponseOut GetDepartmentByIdResponseOut { get; set; }

}

public class GetDepartmentByIdResponseOut
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? DepartmentMangerId { get; set; }
}

