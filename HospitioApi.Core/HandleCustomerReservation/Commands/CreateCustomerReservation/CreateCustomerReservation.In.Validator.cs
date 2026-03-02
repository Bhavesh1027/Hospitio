using FluentValidation;

namespace HospitioApi.Core.HandleCustomerReservation.Commands.CreateCustomerReservation;

public class CreateCustomerReservationValidator : AbstractValidator<CreateCustomerReservationRequest>
{
    public CreateCustomerReservationValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerReservationInValidator());
    }
    public class CreateCustomerReservationInValidator : AbstractValidator<CreateCustomerReservationIn>
    {
        public CreateCustomerReservationInValidator()
        {
            RuleFor(m => m.CustomerId).NotEmpty().NotNull().GreaterThan(0);
            //RuleFor(m => m.Uuid).NotEmpty().NotNull();
            RuleFor(m => m.ReservationNumber).NotEmpty().NotNull();
            //RuleFor(m => m.NoOfGuestAdults).NotEmpty().NotNull();
            //RuleFor(m => m.NoOfGuestChilderns).NotEmpty().NotNull();
            RuleFor(m => m.CheckinDate).NotEmpty().NotNull();
            RuleFor(m => m.CheckoutDate).NotEmpty().NotNull();
            RuleFor(m => m.IsActive).NotEmpty().NotNull().Must(x => x == false || x == true);
        }
    }
}
