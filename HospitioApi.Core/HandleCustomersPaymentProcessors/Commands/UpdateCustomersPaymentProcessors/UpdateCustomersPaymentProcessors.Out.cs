using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.UpdateCustomersPaymentProcessors;

public class UpdateCustomersPaymentProcessorsOut : BaseResponseOut
{
    public UpdateCustomersPaymentProcessorsOut(string message, UpdatedCustomersPaymentProcessorsOut updatedCustomersPaymentProcessorsOut) : base(message)
    {
        UpdatedCustomersPaymentProcessorsOut = updatedCustomersPaymentProcessorsOut;
    }
    public UpdatedCustomersPaymentProcessorsOut UpdatedCustomersPaymentProcessorsOut { get; set; }
}

public class UpdatedCustomersPaymentProcessorsOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? PaymentProcessorId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Currency { get; set; }
}
