using HospitioApi.Shared;

namespace HospitioApi.Core.HandleModules.Queries.GetModulesDapperDemo;

public class GetModulesDapperOut : BaseResponseOut
{
    public GetModulesDapperOut(string message, List<ModulesDapperOut> modulesOut) : base(message)
    {
        ModulesOut = modulesOut;
    }
    public List<ModulesDapperOut> ModulesOut { get; set; } = new List<ModulesDapperOut>();
}
public class ModulesDapperOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public byte? ModuleType { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TotalCount { get; set; }
}
