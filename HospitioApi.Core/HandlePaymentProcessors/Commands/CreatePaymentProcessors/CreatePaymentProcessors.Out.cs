using HospitioApi.Shared;

namespace HospitioApi.Core.HandlePaymentProcessors.Commands.CreatePaymentProcessors;

public class CreatePaymentProcessorsOut : BaseResponseOut
{
    public CreatePaymentProcessorsOut(string message, CreatedPaymentProcessorsOut createdPaymentProcessorsOut) : base(message)
    {
        CreatedPaymentProcessorsOut = createdPaymentProcessorsOut;
    }
    public CreatedPaymentProcessorsOut CreatedPaymentProcessorsOut { get; set; }
}
public class CreatedPaymentProcessorsOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
