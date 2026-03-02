namespace HospitioApi.Core.HandlePaymentDetails.Queries.GetPaymentDetail;

public class AddGuestBuyerIn
{
    public string? external_identifier { get; set; }
    public string? display_name { get;set; }
    public BillingDetails billing_details { get; set; } = new BillingDetails();
}
public class BillingDetails
{
    public string? first_name { get; set; }
    public string? last_name { get; set; }
    public string? email_address { get; set; }
    public string? phone_number { get; set; }
}