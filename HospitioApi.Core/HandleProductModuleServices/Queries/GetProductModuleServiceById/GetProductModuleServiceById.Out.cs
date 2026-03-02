using HospitioApi.Shared;

namespace HospitioApi.Core.HandleProductModuleService.Queries.GetProductModuleServiceById;

public class GetProductModuleServiceByIdOut : BaseResponseOut
{
    public GetProductModuleServiceByIdOut(string message, GetProductModuleServiceByIdResponseOut getProductModuleServiceByIdResponseOut) : base(message)
    {
        GetProductModuleServiceByIdResponseOut = getProductModuleServiceByIdResponseOut;
    }


    public GetProductModuleServiceByIdResponseOut GetProductModuleServiceByIdResponseOut { get; set; }

}

public class GetProductModuleServiceByIdResponseOut
{
    public int Id { get; set; }
    public int? ProductModuleId { get; set; }
    public int? ProductId { get; set; }
    public string? ProductName { get; set; }
    public int? ModuleServiceId { get; set; }
    public string? ModuleServiceName { get; set; }
}

