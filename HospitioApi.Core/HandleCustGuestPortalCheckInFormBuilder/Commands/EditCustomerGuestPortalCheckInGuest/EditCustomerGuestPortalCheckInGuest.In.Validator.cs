using FluentValidation;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Commands.EditCustomerGuestPortalCheckInGuest;

public class EditCustomerGuestPortalCheckInGuestValidator : AbstractValidator<EditCustomerGuestPortalGuestRequest>
{
    public EditCustomerGuestPortalCheckInGuestValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerGuestInValidator());
    }

    public class UpdateCustomerGuestInValidator : AbstractValidator<EditCustomerGuestPortalCheckInGuestIn>
    {
        public UpdateCustomerGuestInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(m => m.Firstname).NotEmpty().NotNull();
            RuleFor(m => m.Lastname).NotEmpty().NotNull();
            RuleFor(m => m.PhoneNumber).NotEmpty().NotNull();
            RuleFor(m => m.Email).NotEmpty().NotNull();
        }
    }
}
