using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Commands.EditCustomerGuestPortalCheckInReservation;

public class EditCustomerGuestPortalCheckInReservationOut: BaseResponseOut
{
    public EditCustomerGuestPortalCheckInReservationOut(string message, UpdatedCustomerReservationOut updatedCustomerReservationOut) : base(message)
    {
        UpdatedCustomerReservationOut = updatedCustomerReservationOut;
    }
    public UpdatedCustomerReservationOut UpdatedCustomerReservationOut { get; set; }
}
public class UpdatedCustomerReservationOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? Uuid { get; set; }
    public string? ReservationNumber { get; set; }
    public string? Source { get; set; }
    public int? NoOfGuestAdults { get; set; }
    public int? NoOfGuestChilderns { get; set; }
    public DateTime? CheckinDate { get; set; }
    public DateTime? CheckoutDate { get; set; }
    public bool? IsActive { get; set; }
}