using HospitioApi.Shared;

namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Commands.DeleteHospitioPaymentProcessors;

public class DeleteHospitioPaymentProcessorsOut : BaseResponseOut
{
    public DeleteHospitioPaymentProcessorsOut(string message, DeletedHospitioPaymentProcessorsOut deletedHospitioPaymentProcessorsOut) : base(message)
    {
        DeletedHospitioPaymentProcessorsOut = deletedHospitioPaymentProcessorsOut;
    }
    public DeletedHospitioPaymentProcessorsOut DeletedHospitioPaymentProcessorsOut { get; set; }
}
public class DeletedHospitioPaymentProcessorsOut
{
    public int Id { get; set; }
    public string ClientId { get; set; } = string.Empty;
}
