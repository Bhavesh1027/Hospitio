using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.EditCustomerGuest;

public class EditCustomerGuestOut : BaseResponseOut
{
    public EditCustomerGuestOut(string message, EditedCustomerGuestOut editedCustomerGuestOut) : base(message)
    {
        EditedCustomerGuestOut = editedCustomerGuestOut;
    }
    public EditedCustomerGuestOut EditedCustomerGuestOut { get; set; }
}
public class EditedCustomerGuestOut
{
    public int Id { get; set; }
    public int? CustomerReservationId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneCountry { get; set; }
    public string PhoneNumber { get; set; }
    public string? Country { get; set; }
    public string? Pin { get; set; }
    public string? Street { get; set; }
    public string? StreetNumber { get; set; }
    public string? City { get; set; }
    public string? Postalcode { get; set; }
    public string? BlePinCode { get; set; }
    public DateTime? CheckinDate { get; set; }
    public DateTime? CheckoutDate { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string UserType { get; set; }
    public int UserId { get; set; }
    public bool? IsActive { get; set; }
}
