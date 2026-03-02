namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtras;

public class DeleteCustomerPropertyExtrasIn
{
    public int Id { get; set; }
}
public class CustomerPropertyExtrasJsonOut
{
    public int Id { get; set; }
    public int? CustomerPropertyInformationId { get; set; }
    public byte? ExtraType { get; set; }
    public string? Name { get; set; }
    public bool IsDeleted { get; set; }
    public List<CustomerPropertyExtraDetailsIn> CustomerPropertyExtraDetailsOuts { get; set; } = new List<CustomerPropertyExtraDetailsIn>();

}
public class CustomerPropertyExtraDetailsIn
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Link { get; set; }
    public bool IsDeleted { get; set; }
}