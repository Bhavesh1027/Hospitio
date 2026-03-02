using FluentValidation;

namespace HospitioApi.Core.HandleAdminCustomerAlerts.Commands.DeleteAdminCustomerAlerts;

public class DeleteAdminCustomerAlertsValidator : AbstractValidator<DeleteAdminCustomerAlertsRequest>
{
    public DeleteAdminCustomerAlertsValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteAdminCustomerAlertsInValidator());
    }
    public class DeleteAdminCustomerAlertsInValidator : AbstractValidator<DeleteAdminCustomerAlertsIn>
    {
        public DeleteAdminCustomerAlertsInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
