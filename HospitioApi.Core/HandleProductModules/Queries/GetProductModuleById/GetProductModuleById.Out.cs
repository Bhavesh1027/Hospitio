using HospitioApi.Shared;

namespace HospitioApi.Core.HandleProductModule.Queries.GetProductModuleById;

public class GetProductModuleByIdOut : BaseResponseOut
{
    public GetProductModuleByIdOut(string message, GetProductModuleByIdResponseOut getProductModuleByIdResponseOut) : base(message)
    {
        GetProductModuleByIdResponseOut = getProductModuleByIdResponseOut;
    }


    public GetProductModuleByIdResponseOut GetProductModuleByIdResponseOut { get; set; }

}

public class GetProductModuleByIdResponseOut
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public int? ModuleId { get; set; }
    public decimal Price { get; set; }
    public string? Currency { get; set; }
    public string? ProductName { get; set; }
    public string? ModuleName { get; set; }
}

