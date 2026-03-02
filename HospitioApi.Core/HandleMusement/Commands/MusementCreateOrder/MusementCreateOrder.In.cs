namespace HospitioApi.Core.HandleMusement.Commands.MusementCreateOrder;

public class MusementCreateOrderIn
{
    public string? Url { get; set; } 
    public string? Cart_uuid {  get; set; }
    public string? Email_notification { get; set; } 
    public string? Guest_Id { get; set; }
    public string? Order_uuid { get; set; }
    public ExtraData? Extra_data { get; set; }
}

public class ExtraData
{
    public string? clientReferenceId { get; set; }
    public string? firstName { get; set; }
    public string? lastName { get; set; }
    public string? reservationId { get; set; }
    public string? utm_campaign { get; set; }
    public string? utm_content { get; set; }
    public string? utm_medium { get; set; }
    public string? utm_source { get; set; }
}
