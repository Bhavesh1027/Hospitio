using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.UpdateCustomerGuest;

public class UpdateCustomerGuestValidator : AbstractValidator<UpdateCustomerGuestRequest>
{
    public UpdateCustomerGuestValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerGuestInValidator());
    }

    public class UpdateCustomerGuestInValidator : AbstractValidator<UpdateCustomerGuestIn>
    {
        public UpdateCustomerGuestInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(m => m.CustomerReservationId).NotEmpty().NotNull().GreaterThan(0);
            //RuleFor(m => m.Firstname).NotEmpty().NotNull();
            //RuleFor(m => m.Lastname).NotEmpty().NotNull();
            //RuleFor(m => m.PhoneNumber).NotEmpty().NotNull();
            //RuleFor(m => m.Email).NotEmpty().NotNull();
            //RuleFor(m => m.Country).NotEmpty().NotNull();
            //RuleFor(m => m.Street).NotEmpty().NotNull();
            //RuleFor(m => m.City).NotEmpty().NotNull();
            //RuleFor(m => m.DateOfBirth).NotEmpty().NotNull();
        }
    }
}
