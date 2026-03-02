namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Commands.CreateCustomerGr4vyPaymentService;

public class AddCustomerDigitalWalletOut
{
    public List<string>? domain_names { get; set; }
    public string? id { get; set; }
    public string? merchant_name { get; set; }
    public string? merchant_url { get; set; }
    public string? provider { get; set; }
}
