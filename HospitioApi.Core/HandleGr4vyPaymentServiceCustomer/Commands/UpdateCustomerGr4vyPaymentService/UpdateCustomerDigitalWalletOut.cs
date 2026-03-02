namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Commands.UpdateCustomerGr4vyPaymentService;

public class UpdateCustomerDigitalWalletOut
{
    public List<string>? domain_names { get; set; }
    public string? id { get; set; }
    public string? merchant_name { get; set; }
    public string? merchant_url { get; set; }
    public string? provider { get; set; }
}
