using FluentValidation;

namespace HospitioApi.Core.HandleCustomerReservation.Commands.UpdateCustomerReservation;

public class UpdateCustomerReservationValidator : AbstractValidator<UpdateCustomerReservationRequest>
{
    public UpdateCustomerReservationValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerReservationInValidator());
    }

    public class UpdateCustomerReservationInValidator : AbstractValidator<UpdateCustomerReservationIn>
    {
        public UpdateCustomerReservationInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().NotNull().GreaterThan(0);
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
