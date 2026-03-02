using HospitioApi.Shared;

namespace HospitioApi.Core.HandleModules.Queries.GetModules;

public class GetModulesOut : BaseResponseOut
{
    public GetModulesOut(string message, List<ModulesOut> modulesOut) : base(message)
    {
        ModulesOut = modulesOut;
    }
    public List<ModulesOut> ModulesOut { get; set; } = new();
}
public class ModulesOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public byte? ModuleType { get; set; }
}
