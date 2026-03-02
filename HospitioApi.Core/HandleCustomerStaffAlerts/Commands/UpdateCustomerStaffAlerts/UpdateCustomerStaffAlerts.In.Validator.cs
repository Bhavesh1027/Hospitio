using FluentValidation;

namespace HospitioApi.Core.HandleCustomerStaffAlerts.Commands.UpdateCustomerStaffAlerts;

public class UpdateCustomerStaffAlertsValidator : AbstractValidator<UpdateCustomerStaffAlertsRequest>
{
    public UpdateCustomerStaffAlertsValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerStaffAlertsInValidator());
    }
    public class UpdateCustomerStaffAlertsInValidator : AbstractValidator<UpdateCustomerStaffAlertsIn>
    {
        public UpdateCustomerStaffAlertsInValidator()
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
