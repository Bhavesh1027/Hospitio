using HospitioApi.Shared;

namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Commands.CreateHospitioPaymentProcessors;

public class CreateHospitioPaymentProcessorsOut : BaseResponseOut
{
    public CreateHospitioPaymentProcessorsOut(string message, CreatedHospitioPaymentProcessorsOut createdHospitioPaymentProcessorsOut) : base(message)
    {
        CreatedHospitioPaymentProcessorsOut = createdHospitioPaymentProcessorsOut;
    }
    public CreatedHospitioPaymentProcessorsOut CreatedHospitioPaymentProcessorsOut { get; set; }
}
public class CreatedHospitioPaymentProcessorsOut
{
    public int Id { get; set; }
    public int? PaymentProcessorId { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
}
