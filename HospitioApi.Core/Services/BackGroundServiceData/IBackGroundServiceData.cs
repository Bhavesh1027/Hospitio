using HospitioApi.Core.HandleVonage.Commands.InboundWebhook;
using HospitioApi.Data;

namespace HospitioApi.Core.Services.BackGroundServiceData;

public interface IBackGroundServiceData
{
    Task<List<CustomerGuestJorneyDetails>> GetGuestMessageByCustomerId(int customerId, DateTime dateTime, CancellationToken cancellationToken);
    Task<List<Customer>> GetCustomers(CancellationToken cancellationToken);
    Task AddAnonymousUser(ApplicationDbContext _db, GetInboundWebhookIn message, CancellationToken stoppingToken);
    Task<List<AlertMessages>> GetAlertMessages(DateTime currentDateTime,CancellationToken stoppingToken);
}
public class AlertMessages
{
    public string? AlertMessage { get; set; }
    public int ChatId {  get; set; }
    public int Minute { get; set; }
    public DateTime? LastMessageSendingTime { get; set; }
    public string? LastMessage { get; set; } 
    public string? AlertType { get; set; }
    public int SenderId { get; set; }
    public int SenderType { get; set; }
    public int ReceiverId { get; set; }
    public int ReceiverType { get; set; }
    public int Platform { get; set; }
    public string? NameOfStaffPerson { get; set; }
    public string? FromPhoneNumber { get; set; }
    public string? ToPhoneCountry { get; set; }
    public string? ToPhoneNumber { get; set; }
    public string? VonageAppId { get; set; }
    public string? VonagePrivateKey { get; set; }
    public string VonageTemplateId { get; set; } = string.Empty;
    public string VonageTemplateStatus { get; set; } = string.Empty;
    public string WhatsappTemplateName { get; set; } = string.Empty;
    public int MsgReqType { get; set; } = 0;

}
public class GuestMessage
{
    public int CustomerUserId { get; set; }
    public int GuestId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string TempletMessage { get; set; } = string.Empty;
    public string ActionName { get; set; } = string.Empty;
}


public class CustomerGuestJorneyDetails
{
    #region Not Rquire Properties
    //public int CustomerUserId { get; set; }
    //public int GuestId { get; set; }
    //public string Email { get; set; } = string.Empty;
    //public string TempletMessage { get; set; } = string.Empty;
    //public string ActionName { get; set; } = string.Empty;
    //public string? JourneyStep { get; set; } = string.Empty;
    //public string? Uuid { get; set; } = string.Empty;
    //public string? Source { get; set; } = string.Empty;
    //public int? NoOfGuestAdults { get; set; }
    //public int? NoOfGuestChildrens { get; set; }
    //public DateTime? CreatedAt { get; set; }
    //public bool? isCheckInCompleted { get; set; }
    //public bool? isSkipCheckIn { get; set; }
    #endregion
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string BussinessName { get; set; } = string.Empty;   
    public string BussinessAddress { get;set; } = string.Empty; 
    public string BussinessPhoneNumber { get;set; } = string.Empty; 
    public string ReservationNumber { get; set; } = string.Empty;
    public DateTime CheckinDate { get; set; }
    public DateTime CheckoutDate { get; set; }
    public int BookingDays { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? TempletMessage { get; set; } = string.Empty;
    public int GuestId { get; set; }
    public string? Buttons { get; set; } = string.Empty;
    public string? VonageTemplateId { get; set; } = string.Empty;
    public string? VonageTemplateStatus { get; set; } = string.Empty;
    public int CustomerUserId { get;set; } 
    public string CustomerWhatsAppNumber { get; set; } = string.Empty;
    public string APPId { get;set; } = string.Empty;    
    public string PrivateKey { get; set; } = string.Empty;
    public string TemplateName { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string ActionName { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
    public string? GuestURL { get; set; } = string.Empty; 
    public string? EligibleForWhatsappCommunication { get; set; } = string.Empty;
    public string? EligibleForSMSCommunication { get; set; } = string.Empty;
    public string? EligibleForEmailCommunication { get;set; } = string.Empty;
    public string? EligibleForWebChatCommunication { get;set; } = string.Empty;
    public string? CustomerLogoURL { get; set; } = string.Empty;
    public string? CustomerColour { get;set; } = string.Empty;
    public string? SMSTwoWayCommunication { get; set; } = string.Empty;

}
public class button
{
    public string type { get; set; } = string.Empty;
    public string text { get; set; } = string.Empty;
    public string value { get; set; } = string.Empty;
}
public class CustomerAction
{
    public int CustomerId { get; set; }
    public string ActionName { get; set; } = string.Empty;
    public int CustomerUserId { get; set; }
}

public class Customer
{
    public int Id { get; set; }
    public int CustomerUserId { get; set; }
}