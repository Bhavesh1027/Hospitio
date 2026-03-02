using HospitioApi.Shared;

namespace HospitioApi.Core.HandleModuleServices.Queries.GetModuleServiceById;

public class GetModuleServiceByIdOut : BaseResponseOut
{
    public GetModuleServiceByIdOut(string message, ModuleServiceById moduleServiceById) : base(message)
    {
        ModuleServiceById = moduleServiceById;
    }
    public ModuleServiceById ModuleServiceById { get; set; }
}
public class ModuleServiceById
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? ModuleId { get; set; }
}
