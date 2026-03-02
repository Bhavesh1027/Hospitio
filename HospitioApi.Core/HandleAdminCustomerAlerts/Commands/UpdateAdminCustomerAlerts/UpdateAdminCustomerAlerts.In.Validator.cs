using FluentValidation;

namespace HospitioApi.Core.HandleAdminCustomerAlerts.Commands.UpdateAdminCustomerAlerts;

public class UpdateAdminCustomerAlertsValidator : AbstractValidator<UpdateAdminCustomerAlertsRequest>
{
    public UpdateAdminCustomerAlertsValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateAdminCustomerAlertsInValidator());
    }

    public class UpdateAdminCustomerAlertsInValidator : AbstractValidator<UpdateAdminCustomerAlertsIn>
    {
        public UpdateAdminCustomerAlertsInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.Msg).NotEmpty();
            RuleFor(m => m.MsgWaitTimeInMinutes).NotEmpty();
        }
    }
}
