using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestPortalCheckInFormBuilder;

public class GetCustomerGuestPortalCheckInFormBuilderOut : BaseResponseOut
{
    public GetCustomerGuestPortalCheckInFormBuilderOut(string message, GetCustomerGuestResponseOut getCustomerGuestsCheckInFormBuilderResponseOut) : base(message)
    {
        GetCustomerReservationResponseOut = getCustomerGuestsCheckInFormBuilderResponseOut;
    }
    public GetCustomerGuestResponseOut GetCustomerReservationResponseOut { get; set; }
}
public class GetCustomerGuestResponseOut
{
    public bool? IsCoGuest { get; set; }
    public string? Name { get; set; }
    public int? BuidlerId { get; set; }
    public int? RoomId { get; set; }
    public string? RoomName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CurrencyCode { get; set; }
    public int? TaxiTransferCommission { get; set; }
    public GetCustomerReservationResponseOut GetCustomerReservationResponseOut { get; set; } = new();
}
public class GetCustomerReservationResponseOut
{
    public int? Id { get; set; }
    public int? CustomerId { get; set; }
    public string? Uuid { get; set; }
    public string? ReservationNumber { get; set; }
    public string? Source { get; set; }
    public int? NoOfGuestAdults { get; set; }
    public int? NoOfGuestChildrens { get; set; }
    public DateTime? CheckinDate { get; set; }
    public DateTime CheckoutDate { get; set; }
    public bool? isCheckInCompleted { get; set; }
    public bool? isSkipCheckIn { get; set; }
    public string? AppAccessCode { get; set; }
    public string? PhoneNumber { get; set; }
    public int? KeyId { get; set; }
    public bool? IsActive { get; set; }
    public GetCustomerGuestsCheckInFormBuilderResponseOut GetCustomerGuestsCheckInFormBuilderResponseOut { get; set; } = new();
}
public class GetCustomerGuestsCheckInFormBuilderResponseOut
{
    public int? Id { set; get; }
    public int? CustomerId { get; set; }
    public string? Color { get; set; }
    public string? Name { get; set; }
    public byte? Stars { get; set; }
    public string? Logo { get; set; }
    public string? AppImage { get; set; }
    public string? SplashScreen { get; set; }
    public bool? IsOnlineCheckInFormEnable { get; set; }
    public bool? IsRedirectToGuestAppEnable { get; set; }
    public string? SubmissionMail { get; set; }
    public string? TermsLink { get; set; }
    public string? GuestWelcomeMessage { get; set; }
    public bool? IsActive { get; set; }
    public string? BusinessType { get; set; }
}