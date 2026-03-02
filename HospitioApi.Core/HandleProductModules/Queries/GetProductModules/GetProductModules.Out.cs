using HospitioApi.Shared;

namespace HospitioApi.Core.HandleProductModule.Queries.GetProductModules;

public class GetProductModulesOut : BaseResponseOut
{
    public GetProductModulesOut(string message, List<GetProductModulesResponseOut> GetProductModulesResponseOut) : base(message)
    {
        getProductModulesResponseOut = GetProductModulesResponseOut;
    }

    public List<GetProductModulesResponseOut> getProductModulesResponseOut { get; set; } = new();

}

public class GetProductModulesResponseOut
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public int? ModuleId { get; set; }
    public decimal Price { get; set; }
    public string? Currency { get; set; }
    public string? ProductName { get; set; }
    public string? ModuleName { get; set; }
}

