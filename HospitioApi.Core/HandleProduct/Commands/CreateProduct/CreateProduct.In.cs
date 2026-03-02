namespace HospitioApi.Core.HandleProduct.Commands.CreateProduct;

public class CreateProductIn
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public bool? IsActive { get; set; }
    public List<ProductModuleRequest>? ProductModules { get; set; }
}

public class ProductModuleRequest
{
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public byte? ModuleType { get; set; }
    public string? Module2TypeValue { get; set; }
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
    public bool IsActive { get; set; } = false;
}