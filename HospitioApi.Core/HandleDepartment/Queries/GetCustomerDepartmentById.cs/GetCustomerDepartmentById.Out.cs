using HospitioApi.Shared;

namespace HospitioApi.Core.HandleDepartment.Queries.GetCustomerDepartmentById.cs;

public class GetCustomerDepartmentByIdOut : BaseResponseOut
{
    public GetCustomerDepartmentByIdOut(string message, GetCustomerDepartmentByIdResponseOut getCustomerDepartmentByIdResponseOut) : base(message)
    {
        GetCustomerDepartmentByIdResponseOut = getCustomerDepartmentByIdResponseOut;
    }

    public GetCustomerDepartmentByIdResponseOut GetCustomerDepartmentByIdResponseOut { get; set; }
}
public class GetCustomerDepartmentByIdResponseOut
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? DepartmentMangerId { get; set; }
}
