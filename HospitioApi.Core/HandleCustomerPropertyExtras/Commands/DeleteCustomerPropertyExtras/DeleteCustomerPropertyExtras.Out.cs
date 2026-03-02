using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtras;

public class DeleteCustomerPropertyExtrasOut : BaseResponseOut
{
    public DeleteCustomerPropertyExtrasOut(string message, DeletedCustomerPropertyExtrasOut deletedCustomerPropertyExtrasOut) : base(message)
    {
        DeletedCustomerPropertyExtrasOut = deletedCustomerPropertyExtrasOut;
    }
    public DeletedCustomerPropertyExtrasOut DeletedCustomerPropertyExtrasOut { get; set; }
}

public class DeletedCustomerPropertyExtrasOut
{
    public int Id { get; set; }
}
