using FluentValidation;

namespace HospitioApi.Core.HandleAdminStaffAlerts.Commands.UpdateAdminStaffAlerts;

public class UpdateAdminStaffAlertsValidator : AbstractValidator<UpdateAdminStaffAlertsRequest>
{
    public UpdateAdminStaffAlertsValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateAdminStaffAlertsInValidator());
    }
    public class UpdateAdminStaffAlertsInValidator : AbstractValidator<UpdateAdminStaffAlertsIn>
    {
        public UpdateAdminStaffAlertsInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.Platfrom).NotEmpty();
            RuleFor(m => m.PhoneCountry).NotEmpty();
            RuleFor(m => m.PhoneNumber).NotEmpty();
            RuleFor(m => m.WaitTimeInMintes).NotEmpty();
        }
    }
}
