using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.DeleteCustomersPaymentProcessors;

public class DeleteCustomersPaymentProcessorsOut : BaseResponseOut
{
    public DeleteCustomersPaymentProcessorsOut(string message, DeletedCustomersPaymentProcessorsOut deletedCustomersPaymentProcessorsOut) : base(message)
    {
        DeletedCustomersPaymentProcessorsOut = deletedCustomersPaymentProcessorsOut;
    }
    public DeletedCustomersPaymentProcessorsOut DeletedCustomersPaymentProcessorsOut { get; set; }
}
public class DeletedCustomersPaymentProcessorsOut
{
    public int Id { get; set; }
}
