using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestAlerts.Commands.UpdateCustomerGuestAlerts;

public class UpdateCustomerGuestAlertsValidator : AbstractValidator<UpdateCustomerGuestAlertsRequest>
{
    public UpdateCustomerGuestAlertsValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerGuestAlertsInValidator());
    }

    public class UpdateCustomerGuestAlertsInValidator : AbstractValidator<UpdateCustomerGuestAlertsIn>
    {
        public UpdateCustomerGuestAlertsInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.OfficeHoursMsg).NotEmpty();
            RuleFor(m => m.OfficeHoursMsgWaitTimeInMinutes).NotEmpty();
            RuleFor(m => m.OfflineHourMsg).NotEmpty();
            RuleFor(M => M.OfflineHoursMsgWaitTimeInMinutes).NotEmpty();
        }
    }
}
