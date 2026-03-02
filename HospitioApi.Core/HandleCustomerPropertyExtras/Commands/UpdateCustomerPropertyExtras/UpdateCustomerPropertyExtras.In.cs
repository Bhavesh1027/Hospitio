namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.UpdateCustomerPropertyExtras;

public class UpdateCustomerPropertyExtrasIn
{
  public List<CustomerPropertyExtrasIn> CustomerPropertyExtrasIns { get; set; } = new List<CustomerPropertyExtrasIn>();
}
public class CustomerPropertyExtrasIn
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

    public List<CustomerPropertyExtraDetailsIn> CustomerPropertyExtraDetailsOuts { get; set; } = new List<CustomerPropertyExtraDetailsIn>();

}
public class CustomerPropertyExtraDetailsIn
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public bool IsDeleted { get; set; }
}
