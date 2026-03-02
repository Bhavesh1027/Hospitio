using FluentValidation;

namespace HospitioApi.Core.HandleCustomerReservation.Commands.DeleteCustomerReservation;

public class DeleteCustomerReservationValidator : AbstractValidator<DeleteCustomerReservationRequest>
{
    public DeleteCustomerReservationValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerReservationInValidator());
    }
    public class DeleteCustomerReservationInValidator : AbstractValidator<DeleteCustomerReservationIn>
    {
        public DeleteCustomerReservationInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
