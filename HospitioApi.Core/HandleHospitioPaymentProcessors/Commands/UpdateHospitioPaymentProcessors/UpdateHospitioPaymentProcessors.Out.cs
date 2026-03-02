using HospitioApi.Shared;

namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Commands.UpdateHospitioPaymentProcessors;

public class UpdateHospitioPaymentProcessorsOut : BaseResponseOut
{
    public UpdateHospitioPaymentProcessorsOut(string message, UpdatedHospitioPaymentProcessorsOut updatedHospitioPaymentProcessorsOut) : base(message)
    {
        UpdatedHospitioPaymentProcessorsOut = updatedHospitioPaymentProcessorsOut;
    }
    public UpdatedHospitioPaymentProcessorsOut UpdatedHospitioPaymentProcessorsOut { get; set; }

}
public class UpdatedHospitioPaymentProcessorsOut
{
    public int Id { get; set; }
    public int PaymentProcessorId { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
}
