using HospitioApi.Core.HandleFeaturePermission.Queries.GetFeaturePermissions;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleFeaturePermission.Queries.GetCustomerFeaturePermissions;

public class GetCustomerFeaturePermissionsOut : BaseResponseOut
{
    public GetCustomerFeaturePermissionsOut(string message, List<GetCustomerFeaturePermissionsResponseOut> getFeaturePermissionsResponseOut) : base(message)
    {
        GetCustomerFeaturePermissionsResponseOut = getFeaturePermissionsResponseOut;
    }
    public List<GetCustomerFeaturePermissionsResponseOut> GetCustomerFeaturePermissionsResponseOut { get; set; } = new();
}
public class GetCustomerFeaturePermissionsResponseOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public bool? IsView { get; set; }
    public bool? IsEdit { get; set; }
    public bool? IsUpload { get; set; }
    public bool? IsReply { get; set; }
    public bool? IsDownload { get; set; }
}