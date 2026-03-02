using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.CreateCustomersPaymentProcessors;

public class CreateCustomersPaymentProcessorsOut : BaseResponseOut
{
    public CreateCustomersPaymentProcessorsOut(string message, CreatedCustomersPaymentProcessorsOut createdCustomersPaymentProcessorsOut) : base(message)
    {
        CreatedCustomersPaymentProcessorsOut = createdCustomersPaymentProcessorsOut;
    }
    public CreatedCustomersPaymentProcessorsOut CreatedCustomersPaymentProcessorsOut { get; set; }
}
public class CreatedCustomersPaymentProcessorsOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? PaymentProcessorId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Currency { get; set; }
}