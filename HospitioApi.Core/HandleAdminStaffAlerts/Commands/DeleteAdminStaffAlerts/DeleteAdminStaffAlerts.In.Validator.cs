using FluentValidation;

namespace HospitioApi.Core.HandleAdminStaffAlerts.Commands.DeleteAdminStaffAlerts;

public class DeleteAdminStaffAlertsValidator : AbstractValidator<DeleteAdminStaffAlertsRequest>
{
    public DeleteAdminStaffAlertsValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteAdminStaffAlertsInValidation());
    }

    public class DeleteAdminStaffAlertsInValidation : AbstractValidator<DeleteAdminStaffAlertsIn>
    {
        public DeleteAdminStaffAlertsInValidation()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
