using HospitioApi.Shared;

namespace HospitioApi.Core.HandleModuleServices.Queries.GetModuleServices;

public class GetModuleServicesOut : BaseResponseOut
{
    public GetModuleServicesOut(string message, List<ModuleServicesOut> moduleServicesOut) : base(message)
    {
        ModuleServicesOut = moduleServicesOut;
    }
    public List<ModuleServicesOut> ModuleServicesOut { get; set; } = new List<ModuleServicesOut>();
}
public class ModuleServicesOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ModuleId { get; set; }
}
