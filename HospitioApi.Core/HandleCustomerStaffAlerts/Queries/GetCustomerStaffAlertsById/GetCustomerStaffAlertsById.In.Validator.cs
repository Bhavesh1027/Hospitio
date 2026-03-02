using FluentValidation;

namespace HospitioApi.Core.HandleCustomerStaffAlerts.Queries.GetCustomerStaffAlertsById;

public class GetCustomerStaffAlertsByIdValidator : AbstractValidator<GetCustomerStaffAlertsByIdRequest>
{
    public GetCustomerStaffAlertsByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerStaffAlertsByIdInValidator());
    }
    public class GetCustomerStaffAlertsByIdInValidator : AbstractValidator<GetCustomerStaffAlertsByIdIn>
    {
        public GetCustomerStaffAlertsByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
