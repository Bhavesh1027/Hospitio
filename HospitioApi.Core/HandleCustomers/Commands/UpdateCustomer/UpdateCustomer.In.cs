namespace HospitioApi.Core.HandleCustomers.Commands.UpdateCustomer;

public class UpdateCustomerIn
{
    public int Id { get; set; }
    public string? BusinessName { get; set; }
    public int? BusinessTypeId { get; set; }
    public int? NoOfRooms { get; set; }
    public string? TimeZone { get; set; }
    public string? WhatsappCountry { get; set; }
    public string? WhatsappNumber { get; set; }
    public string? Cname { get; set; }
    public string? ClientDoamin { get; set; }
    public string? Email { get; set; }
    public string? Messenger { get; set; }
    public string? SmsTitle { get; set; }
    public string? ViberCountry { get; set; }
    public string? ViberNumber { get; set; }
    public string? TelegramCounty { get; set; }
    public string? TelegramNumber { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    //[Column(TypeName = "time(0)")]
    public TimeSpan? BusinessStartTime { get; set; }
    //[Column(TypeName = "time(0)")]
    public TimeSpan? BusinessCloseTime { get; set; }
    public TimeSpan? DoNotDisturbGuestStartTime { get; set; }
    public TimeSpan? DoNotDisturbGuestEndTime { get; set; }
    public bool? StaffAlertsOffduty { get; set; }
    public bool? NoMessageToGuestWhileQuiteTime { get; set; }
    public string? IncomingTranslationLangage { get; set; }
    public string? NoTranslateWords { get; set; }
    public string? CurrencyCode { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsTwoWayComunication { get; set; }
    public byte? PMSId { get; set; }
    public string? PMSAPIAuthUsername { get; set; }
    public string? PMSAPIAuthPassword { get; set; }
    public string? CheckInPolicy {  get; set; }
    public string? CheckOutPolicy { get; set; }
    public string? EmbededEmail {  get; set; }
    public List<UpdateCustomerRoomNamesIn> UpdateCustomerRoomNamesIns { get; set; } = new();

}

public class UpdateCustomerRoomNamesIn
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public byte? LocationType { get; set; }
    public string? CenturionLocationCode { get; set; }
    public bool? IsActive { get; set; }
    public string? Gui { get; set; }
}

