using HospitioApi.Core.HandleUserLevels.Queries.GetUserLevels;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleUserLevels.Queries.GetCustomerLevels;

public class GetCustomerLevelsOut : BaseResponseOut
{
    public GetCustomerLevelsOut(string message, List<CustomerLevelsOut> customerLevelsOut) : base(message)
    {
        CustomerLevelsOut = customerLevelsOut;
    }
    public List<CustomerLevelsOut> CustomerLevelsOut { get; set; } = new();
}
public class CustomerLevelsOut
{
    public int Id { get; set; }
    public string? LevelName { get; set; }
    public string? NormalizedLevelName { get; set; }
}
