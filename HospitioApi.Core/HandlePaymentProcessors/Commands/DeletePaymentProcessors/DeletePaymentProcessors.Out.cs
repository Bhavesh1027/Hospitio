using HospitioApi.Shared;

namespace HospitioApi.Core.HandlePaymentProcessors.Commands.DeletePaymentProcessors;

public class DeletePaymentProcessorsOut : BaseResponseOut
{
    public DeletePaymentProcessorsOut(string message, DeletedPaymentProcessorsOut deletedPaymentProcessorsOut) : base(message)
    {
        DeletedPaymentProcessorsOut = deletedPaymentProcessorsOut;
    }
    public DeletedPaymentProcessorsOut DeletedPaymentProcessorsOut { get; set; }
}
public class DeletedPaymentProcessorsOut
{
    public int Id { get; set; }
}
