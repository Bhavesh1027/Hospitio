using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtraDetail;

public class DeleteCustomerPropertyExtraDetailOut: BaseResponseOut
{
    public DeleteCustomerPropertyExtraDetailOut(string message, DeletedCustomerPropertyExtraDetailOut deleted) : base(message)
    {
        deletedCustomer = deleted;
    }
    public DeletedCustomerPropertyExtraDetailOut deletedCustomer { get; set; }
}
public class DeletedCustomerPropertyExtraDetailOut
{
    public int Id { get; set; }
}