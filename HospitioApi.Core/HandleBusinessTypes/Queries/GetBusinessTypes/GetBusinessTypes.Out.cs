using HospitioApi.Shared;

namespace HospitioApi.Core.HandleBusinessTypes.Queries.GetBusinessTypes;

public class GetBusinessTypesOut : BaseResponseOut
{
    public GetBusinessTypesOut(string message, List<BusinessTypesOut> businessTypesOut) : base(message)
    {
        BusinessTypesOut = businessTypesOut;
    }
    public List<BusinessTypesOut> BusinessTypesOut { get; set; } = new();

}
public class BusinessTypesOut
{
    public int Id { get; set; }
    public string BizType { get; set; } = string.Empty;
    public bool IsActive { get; set; }

}
