using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.CreateCustomerGuest;

public class CreateCustomerGuestValidator : AbstractValidator<CreateCustomerGuestRequest>
{
    public CreateCustomerGuestValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerGuestInValidator());
    }
    public class CreateCustomerGuestInValidator : AbstractValidator<CreateCustomerGuestIn>
    {
        public CreateCustomerGuestInValidator()
        {
            RuleFor(m => m.CustomerReservationId).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(m => m.Firstname).NotEmpty().NotNull();
            RuleFor(m => m.Lastname).NotEmpty().NotNull();
            RuleFor(m => m.PhoneNumber).NotEmpty().NotNull();
            RuleFor(m => m.Email).NotEmpty().NotNull();
            //RuleFor(m => m.Country).NotEmpty().NotNull();
            //RuleFor(m => m.Street).NotEmpty().NotNull();
            //RuleFor(m => m.City).NotEmpty().NotNull();
            //RuleFor(m => m.DateOfBirth).NotEmpty().NotNull();
        }
    }
}
