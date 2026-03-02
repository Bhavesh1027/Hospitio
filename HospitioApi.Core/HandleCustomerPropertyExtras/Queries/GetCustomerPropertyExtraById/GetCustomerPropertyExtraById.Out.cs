using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Queries.GetCustomerPropertyExtraById;

public class GetCustomerPropertyExtraByIdOut : BaseResponseOut
{
    public GetCustomerPropertyExtraByIdOut(string message, CustomerPropertyExtraByIdOut customerPropertyExtraByIdOut) : base(message)
    {
        CustomerPropertyExtraByIdOut = customerPropertyExtraByIdOut;
    }

    public CustomerPropertyExtraByIdOut CustomerPropertyExtraByIdOut { get; set; }
}

public class CustomerPropertyExtraByIdOut
{
    public int Id { get; set; }
    public int? CustomerPropertyInformationId { get; set; }
    /// <summary>
    /// 1: Recommendation,  2: Around You
    /// </summary>
    public byte? ExtraType { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Link { get; set; }
}
