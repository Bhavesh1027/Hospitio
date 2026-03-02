using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuest.Queries.GetMainGuestByReservationId;

public class GetMainGuestByReservationIdValidator : AbstractValidator<GetCustomerGuestByReservationIdRequest>
{
    public GetMainGuestByReservationIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerGuestByReservationIdInValidator());

    }
    public class GetCustomerGuestByReservationIdInValidator : AbstractValidator<GetMainGuestByReservationIdIn>
    {
        public GetCustomerGuestByReservationIdInValidator()
        {
            RuleFor(m => m.ReservationId).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
