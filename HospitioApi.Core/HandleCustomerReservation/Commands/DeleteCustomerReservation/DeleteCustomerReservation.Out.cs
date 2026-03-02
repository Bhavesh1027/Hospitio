using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerReservation.Commands.DeleteCustomerReservation;

public class DeleteCustomerReservationOut : BaseResponseOut
{
    public DeleteCustomerReservationOut(string message, DeletedCustomerReservationOut deletedCustomerReservationOut) : base(message)
    {

        DeletedCustomerReservationOut = deletedCustomerReservationOut;
    }
    public DeletedCustomerReservationOut DeletedCustomerReservationOut { get; set; }
}
public class DeletedCustomerReservationOut
{
    public int Id { get; set; }
}
