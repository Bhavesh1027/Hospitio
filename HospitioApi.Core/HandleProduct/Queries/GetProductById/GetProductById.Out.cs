using HospitioApi.Shared;

namespace HospitioApi.Core.HandleProduct.Queries.GetProductById;

public class GetProductByIdOut : BaseResponseOut
{
    public GetProductByIdOut(string message, GetProductByIdResponseOut getProductByIdResponseOut) : base(message)
    {
        GetProductByIdResponseOut = getProductByIdResponseOut;
    }

    public GetProductByIdResponseOut GetProductByIdResponseOut { get; set; }

}

public class GetProductByIdResponseOut
{
    public int? ProductId { get; set; }
    public string? ProductName { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; }
    public List<ProductModule> ProductModules { get; set; } = new();

}


public class ProductModule
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public int? ModuleId { get; set; }
    public string? ModuleName { get; set; }
    public string? Module2TypeValue { get; set; }
    public byte? ModuleType { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public bool IsActive { get; set; } = false;
    public List<ProductModuleService> ProductModuleServices { get; set; } = new();
}


public class ProductModuleService
{
    public int Id { get; set; }
    public int? ProductModuleId { get; set; }
    public int? ProductId { get; set; }
    public int? ModuleServiceId { get; set; }
    public string? ModuleServiceName { get; set; }
    public bool IsActive { get; set; } = false;
}