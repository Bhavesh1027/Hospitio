using FluentValidation;

namespace HospitioApi.Core.HandleCustomerStaffAlerts.Commands.DeleteCustomerStaffAlerts;

public class DeleteCustomerStaffAlertsValidation : AbstractValidator<DeleteCustomerStaffAlertsRequest>
{
    public DeleteCustomerStaffAlertsValidation()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerStaffAlertsInValidation());
    }

    public class DeleteCustomerStaffAlertsInValidation : AbstractValidator<DeleteCustomerStaffAlertsIn>
    {
        public DeleteCustomerStaffAlertsInValidation()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
