using FluentValidation;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestPortalCheckInFormBuilder;

public class GetCustomerGuestPortalCheckInFormBuilderValidator : AbstractValidator<GetCustomerGuestPortalCheckInFormBuilderRequest>
{
    public GetCustomerGuestPortalCheckInFormBuilderValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerGuestsCheckInFormBuilderInValidator());
    }
    public class GetCustomerGuestsCheckInFormBuilderInValidator : AbstractValidator<GetCustomerGuestPortalCheckInFormBuilderIn>
    {
        public GetCustomerGuestsCheckInFormBuilderInValidator()
        {
        }
    }
}
