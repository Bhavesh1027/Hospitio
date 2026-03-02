namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtraDetail;

public class DeleteCustomerPropertyExtraDetailIn
{
    public int Id { get; set; }
}
public class CustomerPropertyExtraDetailJsonOut
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Link { get; set; }
    public bool IsDeleted { get; set; }
}