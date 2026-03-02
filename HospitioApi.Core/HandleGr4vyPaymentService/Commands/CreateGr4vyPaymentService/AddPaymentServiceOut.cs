namespace HospitioApi.Core.HandleGr4vyPaymentService.Commands.CreateGr4vyPaymentService;

public class AddPaymentServiceOut
{
    public string? Type { get; set; }
    public string? Id { get; set; }
    public string? Merchant_Account_Id { get; set; }
    public string? Payment_Service_Definition_Id { get; set; }
    public bool? Active { get; set; }
    public string? Method { get; set; }
    public string? Display_Name { get; set; }
    public int? Position { get; set; }
    public string? Status { get; set; }
    public List<string>? Accepted_Currencies { get; set; }
    public List<string>? Accepted_Countries { get; set; }
    public DateTime? Created_At { get; set; }
    public DateTime? Updated_At { get; set; }
    public bool? Payment_Method_Tokenization_Enabled { get; set; }
    public bool? Network_Tokens_Enabled { get; set; }
    public bool? Open_Loop { get; set; }
    public bool Three_D_Secure_Enabled { get; set; }
    public object? Merchant_Profile { get; set; }
    public string? Webhook_Url { get; set; }
    public List<Field>? Fields { get; set; }
    public bool Is_Deleted { get; set; }
}
