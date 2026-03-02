using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitioApi.Data.Models;


public partial class Customer : Auditable
{
    public Customer()
    {
        CustomerDigitalAssistants = new HashSet<CustomerDigitalAssistant>();
        CustomerGuestAlerts = new HashSet<CustomerGuestAlert>();
        CustomerGuestAppBuilders = new HashSet<CustomerGuestAppBuilder>();
        CustomerGuestAppConciergeCategories = new HashSet<CustomerGuestAppConciergeCategory>();
        CustomerGuestAppConciergeItems = new HashSet<CustomerGuestAppConciergeItem>();
        CustomerGuestAppEnhanceYourStayCategories = new HashSet<CustomerGuestAppEnhanceYourStayCategory>();
        CustomerGuestAppEnhanceYourStayItems = new HashSet<CustomerGuestAppEnhanceYourStayItem>();
        CustomerGuestAppHousekeepingCategories = new HashSet<CustomerGuestAppHousekeepingCategory>();
        CustomerGuestAppHousekeepingItems = new HashSet<CustomerGuestAppHousekeepingItem>();
        CustomerGuestAppReceptionCategories = new HashSet<CustomerGuestAppReceptionCategory>();
        CustomerGuestAppReceptionItems = new HashSet<CustomerGuestAppReceptionItem>();
        CustomerGuestAppRoomServiceCategories = new HashSet<CustomerGuestAppRoomServiceCategory>();
        CustomerGuestAppRoomServiceItems = new HashSet<CustomerGuestAppRoomServiceItem>();
        CustomerGuestJournies = new HashSet<CustomerGuestJourny>();
        CustomerGuestsCheckInFormBuilders = new HashSet<CustomerGuestsCheckInFormBuilder>();
        CustomerGuestsCheckInFormFields = new HashSet<CustomerGuestsCheckInFormField>();
        CustomerPaymentProcessors = new HashSet<CustomerPaymentProcessor>();
        CustomerPropertyInformations = new HashSet<CustomerPropertyInformation>();
        CustomerReservations = new HashSet<CustomerReservation>();
        CustomerRoomNames = new HashSet<CustomerRoomName>();
        CustomerStaffAlerts = new HashSet<CustomerStaffAlert>();
        CustomerUsers = new HashSet<CustomerUser>();
        GuestRequests = new HashSet<GuestRequest>();
        //  NotificationHistories = new HashSet<NotificationHistory>();
        Tickets = new HashSet<Ticket>();
        CustomerPaymentProcessorCredentials = new HashSet<CustomerPaymentProcessorCredentials>();
        CustomerDepartments = new HashSet<CustomerDepartment>();
        CustomerGroups = new HashSet<CustomerGroup>();
        ChatWidgetUsers = new HashSet<ChatWidgetUser>();
        TaxiTransferGuestRequests = new HashSet<TaxiTransferGuestRequest>();
    }

    public Guid Guid { get; set; }

    [MaxLength(100)]
    public string? BusinessName { get; set; }
    public int? BusinessTypeId { get; set; }
    public int? NoOfRooms { get; set; }
    [MaxLength(50)]
    public string? TimeZone { get; set; }
    [MaxLength(3)]
    public string? WhatsappCountry { get; set; }
    [MaxLength(20)]
    public string? WhatsappNumber { get; set; }
    [MaxLength(250)]
    public string? Cname { get; set; }
    [MaxLength(50)]
    public string? ClientDoamin { get; set; }
    [MaxLength(100)]
    public string? Email { get; set; }
    [MaxLength(200)]
    public string? SmsTitle { get; set; }
    [MaxLength(100)]
    public string? Messenger { get; set; }
    [MaxLength(3)]
    public string? ViberCountry { get; set; }
    [MaxLength(20)]
    public string? ViberNumber { get; set; }
    [MaxLength(3)]
    public string? TelegramCounty { get; set; }
    [MaxLength(20)]
    public string? TelegramNumber { get; set; }
    [MaxLength(3)]
    public string? PhoneCountry { get; set; }
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    public bool? IsTwoWayComunication { get; set; }
    [Column(TypeName = "time(0)")]
    public TimeSpan? BusinessStartTime { get; set; }
    [Column(TypeName = "time(0)")]
    public TimeSpan? BusinessCloseTime { get; set; }
    [Column(TypeName = "time(0)")]
    public TimeSpan? DoNotDisturbGuestStartTime { get; set; }
    [Column(TypeName = "time(0)")]
    public TimeSpan? DoNotDisturbGuestEndTime { get; set; }
    public bool? StaffAlertsOffduty { get; set; }
    public bool? NoMessageToGuestWhileQuiteTime { get; set; }
    [MaxLength(10)]
    public string? IncomingTranslationLangage { get; set; }
    public string? NoTranslateWords { get; set; }
    public int? ProductId { get; set; }
    public DateTime? DeActivated { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Postalcode { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }

    /// <summary>
    /// 1=Centurion, 2=GEA
    /// </summary>
    public byte? GuidType { get; set; }
    [MaxLength(100)]
    public string? Vat { get; set; }
    [MaxLength(3)]
    public string? CurrencyCode { get; set; }
    public DateTime? SubscriptionExpirationDate { get; set; }
    public string? WidgetChatId { get; set; }
    /// <summary>
    /// 1=None, 2=SiteMinder, 3=Avantio
    /// </summary>

    [DefaultValue(1)]
    public byte? PMSId { get; set; }
    public string? PMSAPIAuthUsername { get; set; }
    public string? PMSAPIAuthPassword { get; set; }
    public bool? IsPylonGenerated { get; set; }
    public string? CheckInPolicy { get; set; }
    public string? CheckOutPolicy {  get; set; }
    public string? EmbededEmail { get; set; }
    public virtual BusinessType? BusinessType { get; set; }
    public virtual Product? Product { get; set; }
    public virtual ICollection<CustomerDigitalAssistant> CustomerDigitalAssistants { get; set; }
    public virtual ICollection<CustomerGuestAlert> CustomerGuestAlerts { get; set; }
    public virtual ICollection<CustomerGuestAppBuilder> CustomerGuestAppBuilders { get; set; }
    public virtual ICollection<CustomerGuestAppConciergeCategory> CustomerGuestAppConciergeCategories { get; set; }
    public virtual ICollection<CustomerGuestAppConciergeItem> CustomerGuestAppConciergeItems { get; set; }
    public virtual ICollection<CustomerGuestAppEnhanceYourStayCategory> CustomerGuestAppEnhanceYourStayCategories { get; set; }
    public virtual ICollection<CustomerGuestAppEnhanceYourStayItem> CustomerGuestAppEnhanceYourStayItems { get; set; }
    public virtual ICollection<CustomerGuestAppHousekeepingCategory> CustomerGuestAppHousekeepingCategories { get; set; }
    public virtual ICollection<CustomerGuestAppHousekeepingItem> CustomerGuestAppHousekeepingItems { get; set; }
    public virtual ICollection<CustomerGuestAppReceptionCategory> CustomerGuestAppReceptionCategories { get; set; }
    public virtual ICollection<CustomerGuestAppReceptionItem> CustomerGuestAppReceptionItems { get; set; }
    public virtual ICollection<CustomerGuestAppRoomServiceCategory> CustomerGuestAppRoomServiceCategories { get; set; }
    public virtual ICollection<CustomerGuestAppRoomServiceItem> CustomerGuestAppRoomServiceItems { get; set; }
    public virtual ICollection<CustomerGuestJourny> CustomerGuestJournies { get; set; }
    public virtual ICollection<CustomerGuestsCheckInFormBuilder> CustomerGuestsCheckInFormBuilders { get; set; }
    public virtual ICollection<CustomerGuestsCheckInFormField> CustomerGuestsCheckInFormFields { get; set; }
    public virtual ICollection<CustomerPaymentProcessor> CustomerPaymentProcessors { get; set; }
    public virtual ICollection<CustomerPropertyInformation> CustomerPropertyInformations { get; set; }
    public virtual ICollection<CustomerReservation> CustomerReservations { get; set; }
    public virtual ICollection<CustomerRoomName> CustomerRoomNames { get; set; }
    public virtual ICollection<CustomerStaffAlert> CustomerStaffAlerts { get; set; }
    public virtual ICollection<CustomerUser> CustomerUsers { get; set; }
    public virtual ICollection<GuestRequest> GuestRequests { get; set; }
    //  public virtual ICollection<NotificationHistory> NotificationHistories { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; }
    public virtual ICollection<CustomerPaymentProcessorCredentials> CustomerPaymentProcessorCredentials { get; set; }
    public virtual ICollection<CustomerDepartment> CustomerDepartments { get; set; }
    public virtual ICollection<CustomerGroup> CustomerGroups { get; set; }
    public virtual ICollection<ChatWidgetUser> ChatWidgetUsers { get; set; }
    public virtual ICollection<TaxiTransferGuestRequest> TaxiTransferGuestRequests { get; set; }
}
