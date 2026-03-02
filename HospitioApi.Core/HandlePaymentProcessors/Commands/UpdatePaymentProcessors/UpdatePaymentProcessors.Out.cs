using HospitioApi.Shared;

namespace HospitioApi.Core.HandlePaymentProcessors.Commands.UpdatePaymentProcessors;

public class UpdatePaymentProcessorsOut : BaseResponseOut
{
    public UpdatePaymentProcessorsOut(string message, UpdatedPaymentProcessorsOut updatedPaymentProcessorsOut) : base(message)
    {
        UpdatedPaymentProcessorsOut = updatedPaymentProcessorsOut;
    }
    public UpdatedPaymentProcessorsOut UpdatedPaymentProcessorsOut { get; set; }
}
public class UpdatedPaymentProcessorsOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
