using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestsByReservation;

public class GetCustomerGuestsByReservationOut : BaseResponseOut
{
    public GetCustomerGuestsByReservationOut(string message, List<CustomerGuestsByReservationOut> customerGuestsOut) : base(message)
    {
        CustomerGuestsOut = customerGuestsOut;
    }
    public List<CustomerGuestsByReservationOut> CustomerGuestsOut { get; set; }
}
public class CustomerGuestsByReservationOut
{
    public int Id { get; set; }
    public int? CustomerReservationId { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public string? Picture { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public byte? AgeCategory { get; set; }
    public bool? IsCoGuest { get; set; }
    public bool? isCheckInCompleted { get; set; }

}