using FluentValidation;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Commands.EditCustomerGuestPortalCheckInReservation;

public class EditCustomerGuestPortalCheckInReservationValidator : AbstractValidator<EditGuestAppCustomerReservationRequest>
{
    public EditCustomerGuestPortalCheckInReservationValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerReservationInValidator());
    }

    public class UpdateCustomerReservationInValidator : AbstractValidator<EditCustomerGuestPortalCheckInReservationIn>
    {
        public UpdateCustomerReservationInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().NotNull().GreaterThan(0);
            //RuleFor(m => m.NoOfGuestAdults).NotEmpty().NotNull();
            //RuleFor(m => m.NoOfGuestChilderns).NotEmpty().NotNull();
        }
    }
}
