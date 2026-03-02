using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerReservation.Commands.TransferCustomerGuestReservation;

public class TransferCustomerGuestReservationOut: BaseResponseOut
{
    public TransferCustomerGuestReservationOut(string message, TransferedCustomerReservationOut updatedCustomerReservationOut) : base(message)
    {
        UpdatedCustomerReservationOut = updatedCustomerReservationOut;
    }
    public TransferedCustomerReservationOut UpdatedCustomerReservationOut { get; set; }
}
public class TransferedCustomerReservationOut
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