using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestAlerts.Commands.DeleteCustomerGuestAlerts;

public class DeleteCustomerGuestAlertsValidator : AbstractValidator<DeleteCustomerGuestAlertsRequest>
{
    public DeleteCustomerGuestAlertsValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerGuestAlertsInValidator());
    }
    public class DeleteCustomerGuestAlertsInValidator : AbstractValidator<DeleteCustomerGuestAlertsIn>
    {
        public DeleteCustomerGuestAlertsInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
