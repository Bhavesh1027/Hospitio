using FluentValidation;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Commands.CreateCustomerGuestPortalCheckInFormBuilder;

public class CreateCustomerGuestPortalCheckInFormBuilderValidator : AbstractValidator<CreateCustomerGuestPortalCheckInFormBuilderRequest>
{
    public CreateCustomerGuestPortalCheckInFormBuilderValidator()
    {
        RuleFor(m => m.In).SetValidator(new CustomerGuestsCheckInFormBuilderInValidator());
    }
    public class CustomerGuestsCheckInFormBuilderInValidator : AbstractValidator<CreateCustomerGuestPortalCheckInFormBuilderIn>
    {
        public CustomerGuestsCheckInFormBuilderInValidator()
        {
        }
    }
}
