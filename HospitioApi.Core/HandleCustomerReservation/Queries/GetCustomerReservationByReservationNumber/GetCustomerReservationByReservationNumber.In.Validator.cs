using FluentValidation;

namespace HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationByReservationNumber;

public class GetCustomerReservationByReservationNumberValidator : AbstractValidator<GetCustomerReservationByNumberRequest>
{
    public GetCustomerReservationByReservationNumberValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerReservationByNumberInValidator());

    }
    public class GetCustomerReservationByNumberInValidator : AbstractValidator<GetCustomerReservationByReservationNumberIn>
    {
        public GetCustomerReservationByNumberInValidator()
        {
            RuleFor(m => m.ReservationNumber).NotEmpty().NotNull();
        }
    }
}
