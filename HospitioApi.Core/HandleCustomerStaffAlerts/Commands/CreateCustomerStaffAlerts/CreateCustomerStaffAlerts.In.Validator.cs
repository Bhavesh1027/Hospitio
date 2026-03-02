using FluentValidation;

namespace HospitioApi.Core.HandleCustomerStaffAlerts.Commands.CreateCustomerStaffAlerts;

public class CreateCustomerStaffAlertsValidator : AbstractValidator<CreateCustomerStaffAlertsRequest>
{
    public CreateCustomerStaffAlertsValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerStaffAlertsInValidator());
    }
    public class CreateCustomerStaffAlertsInValidator :
        AbstractValidator<CreateCustomerStaffAlertsIn>
    {
        public CreateCustomerStaffAlertsInValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.Platfrom).NotEmpty();
            RuleFor(m => m.PhoneCountry).NotEmpty();
            RuleFor(m => m.PhoneNumber).NotEmpty();
            RuleFor(m => m.WaitTimeInMintes).NotEmpty();
        }
    }
}
