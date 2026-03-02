using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Queries.GetCustomerPropertyExtras;

public class GetCustomerPropertyExtrasOut : BaseResponseOut
{
    public GetCustomerPropertyExtrasOut(string message, List<CustomerPropertyExtraOut> customerPropertyExtra) : base(message)
    {
        CustomerPropertyExtra = customerPropertyExtra;
    }

    public List<CustomerPropertyExtraOut> CustomerPropertyExtra { get; set; } = new();

}

public class CustomerPropertyExtraOut
{
    public int Id { get; set; }
    public int? CustomerPropertyInformationId { get; set; }
    /// <summary>
    /// 1: Recommendation,  2: Around You
    /// </summary>
    public byte? ExtraType { get; set; }
    public string? Name { get; set; }
    public bool IsDeleted { get; set; }
    public int? DisplayOrder { get; set; }

    public List<CustomerPropertyExtraDetailsOut> customerPropertyExtraDetailsOuts { get; set; } = new();
    
}
public class CustomerPropertyExtraDetailsOut
{
    public int Id { get; set; }
    public int CustomerPropertyExtraId { get; set; }
    public string? Description { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public bool IsDeleted { get; set; }
}