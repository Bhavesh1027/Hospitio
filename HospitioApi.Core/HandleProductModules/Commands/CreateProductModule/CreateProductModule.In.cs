namespace HospitioApi.Core.HandleProductModule.Commands.CreateProductModule;

public class CreateProductModuleIn
{
    public int ProductId { get; set; }
    public int ModuleId { get; set; }
    public decimal Price { get; set; }
    public string? Currency { get; set; }
}


