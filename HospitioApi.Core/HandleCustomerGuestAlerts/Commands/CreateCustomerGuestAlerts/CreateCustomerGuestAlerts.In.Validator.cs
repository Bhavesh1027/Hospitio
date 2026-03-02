using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestAlerts.Commands.CreateCustomerGuestAlerts;

public class CreateCustomerGuestAlertsValidator : AbstractValidator<CreateCustomerGuestAlertsRequest>
{
    public CreateCustomerGuestAlertsValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerGuestAlertsInValidator());
    }
    public class CreateCustomerGuestAlertsInValidator : AbstractValidator<CreateCustomerGuestAlertsIn>
    {
        public CreateCustomerGuestAlertsInValidator()
        {
            RuleFor(m => m.OfficeHoursMsg).NotEmpty();
            RuleFor(m => m.OfficeHoursMsgWaitTimeInMinutes).NotEmpty();
            RuleFor(m => m.OfflineHourMsg).NotEmpty();
            RuleFor(M => M.OfflineHoursMsgWaitTimeInMinutes).NotEmpty();
        }
    }
}
