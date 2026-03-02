namespace HospitioApi.Core.HandleProductModule.Commands.EditProductModule;

public class EditProductModuleIn
{
    public int ProductId { get; set; }
    public int ModuleId { get; set; }
    public decimal Price { get; set; }
    public string? Currency { get; set; }
}


