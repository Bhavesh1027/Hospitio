using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.EditCustomerGuest;

public class EditCustomerGuestValidator : AbstractValidator<EditCustomerGuestRequest>
{
    public EditCustomerGuestValidator()
    {
        RuleFor(m => m.In).SetValidator(new EditCustomerGuestInValidator());
    }

    public class EditCustomerGuestInValidator : AbstractValidator<EditCustomerGuestIn>
    {
        public EditCustomerGuestInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().NotNull().GreaterThan(0);
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

