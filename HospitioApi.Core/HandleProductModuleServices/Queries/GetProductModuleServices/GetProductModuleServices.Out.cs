using HospitioApi.Shared;

namespace HospitioApi.Core.HandleProductModuleService.Queries.GetProductModuleServices;

public class GetProductModuleServicesOut : BaseResponseOut
{
    public GetProductModuleServicesOut(string message, List<GetProductModuleServicesResponseOut> GetProductModuleServicesResponseOut) : base(message)
    {
        getProductModuleServicesResponseOut = GetProductModuleServicesResponseOut;
    }

    public List<GetProductModuleServicesResponseOut> getProductModuleServicesResponseOut { get; set; } = new List<GetProductModuleServicesResponseOut>();

}

public class GetProductModuleServicesResponseOut
{
    public int Id { get; set; }
    public int? ProductModuleId { get; set; }
    public int? ProductId { get; set; }
    public string? ProductName { get; set; }
    public int? ModuleServiceId { get; set; }
    public string? ModuleServiceName { get; set; }
}

