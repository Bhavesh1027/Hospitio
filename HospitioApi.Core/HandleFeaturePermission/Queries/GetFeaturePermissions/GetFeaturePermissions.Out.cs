using HospitioApi.Shared;

namespace HospitioApi.Core.HandleFeaturePermission.Queries.GetFeaturePermissions;
public class GetFeaturePermissionsOut : BaseResponseOut
{
    public GetFeaturePermissionsOut(string message, List<GetFeaturePermissionsResponseOut> getFeaturePermissionsResponseOut) : base(message)
    {
        GetFeaturePermissionsResponseOut = getFeaturePermissionsResponseOut;
    }

    public List<GetFeaturePermissionsResponseOut> GetFeaturePermissionsResponseOut { get; set; } = new();

}

public class GetFeaturePermissionsResponseOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool? IsView { get; set; }
    public bool? IsEdit { get; set; }
    public bool? IsReply { get; set; }
    public bool? IsUpload { get; set; }
    public bool? IsSend { get; set; }
}
