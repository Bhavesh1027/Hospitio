using HospitioApi.Shared;

namespace HospitioApi.Core.HandleUserAccount.Queries.GetUsersByGroupId;

public class GetUsersByGroupIdOut : BaseResponseOut
{
    public GetUsersByGroupIdOut(string message, List<UsersByGroupIdOut> deptWiseUserOut) : base(message)
    {
        DeptWiseUserOut = deptWiseUserOut;
    }
    public List<UsersByGroupIdOut> DeptWiseUserOut { get; set; } = new();
}
public class UsersByGroupIdOut
{
    public int? Id { get; set; }
    public string? Name { get; set; }

}