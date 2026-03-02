namespace HospitioApi.Core.HandlePaymentDetails.Queries.GetPaymentDetail;

public class AddGuestBuyerOut
{
    public string? type { get; set; }
    public string? id { get; set; }
    public string? merchant_account_id { get; set; }
    public string? external_identifier { get; set; }
    public string? display_name { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
}

