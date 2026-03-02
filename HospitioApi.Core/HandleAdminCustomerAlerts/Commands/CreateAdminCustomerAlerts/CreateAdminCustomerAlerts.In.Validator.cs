using FluentValidation;

namespace HospitioApi.Core.HandleAdminCustomerAlerts.Commands.CreateAdminCustomerAlerts;

public class CreateAdminCustomerAlertsValidator : AbstractValidator<CreateAdminCustomerAlertsRequest>
{
    public CreateAdminCustomerAlertsValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateAdminCustomerAlertsInValidator());
    }
    public class CreateAdminCustomerAlertsInValidator : AbstractValidator<CreateAdminCustomerAlertsIn>
    {
        public CreateAdminCustomerAlertsInValidator()
        {
            RuleFor(m => m.Msg).NotEmpty();
            RuleFor(m => m.MsgWaitTimeInMinutes).NotEmpty();
        }
    }
}
