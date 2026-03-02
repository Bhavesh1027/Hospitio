using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGr4vyPaymentService.Queries.GetGr4vyPaymentService;

public class GetGr4vyPaymentServiceOut : BaseResponseOut
{
    public GetGr4vyPaymentServiceOut(string message, List<HospitioPaymentServicesOut> paymentServicesOuts) : base(message)
    {
        hospitioPaymentServices = paymentServicesOuts;
    }
    public List<HospitioPaymentServicesOut> hospitioPaymentServices { get; set; } = new();
}
public class HospitioPaymentServicesOut
{
    public int? HospitioPaymentProcessorId { get; set; }
    public bool? IsActive { get; set; }
    public string? GRCategory { get; set; }
    public string? GRGroup { get; set; }
    public string? GRIcon { get; set; }
    public string? GRName { get; set; }

}