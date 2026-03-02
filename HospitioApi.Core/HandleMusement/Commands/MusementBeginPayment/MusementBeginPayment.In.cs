namespace HospitioApi.Core.HandleMusement.Commands.MusementBeginPaymentStripe;

public class MusementBeginPaymentIn
{
    public string Url { get; set; } = string.Empty;
    public string adyen_token { get; set; } = string.Empty;
    public string card_brand { get; set; } = string.Empty;
    public string card_country { get; set; } = string.Empty;
    public string client_ip { get; set; } = string.Empty;
    public string order_uuid { get; set; } = string.Empty;
    public string redirect_url_success_3d_secure { get; set; } = string.Empty;
}
