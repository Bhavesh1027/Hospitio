using FluentValidation;

namespace HospitioApi.Core.HandleAdminStaffAlerts.Commands.CreateAdminStaffAlerts;

public class CreateAdminStaffAlertsValidator : AbstractValidator<CreateAdminStaffAlertsRequest>
{
    public CreateAdminStaffAlertsValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateAdminStaffAlertsInValidator());
    }
    public class CreateAdminStaffAlertsInValidator :
        AbstractValidator<CreateAdminStaffAlertsIn>
    {
        public CreateAdminStaffAlertsInValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.Platfrom).NotEmpty();
            RuleFor(m => m.PhoneCountry).NotEmpty();
            RuleFor(m => m.PhoneNumber).NotEmpty();
            RuleFor(m => m.WaitTimeInMintes).NotEmpty();
        }
    }
}
