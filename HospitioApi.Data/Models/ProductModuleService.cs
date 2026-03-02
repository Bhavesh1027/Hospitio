namespace HospitioApi.Data.Models;

public partial class ProductModuleService : Auditable
{
    public int? ProductModuleId { get; set; }
    public int? ProductId { get; set; }
    public int? ModuleServiceId { get; set; }

    public virtual ProductModule? ProductModule { get; set; }
    public virtual ModuleService? ModuleService { get; set; }
    public virtual Product? Product { get; set; }
}
