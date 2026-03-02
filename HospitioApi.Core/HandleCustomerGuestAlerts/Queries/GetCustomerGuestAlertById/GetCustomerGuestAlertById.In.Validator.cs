using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestAlerts.Queries.GetCustomerGuestAlertById;

public class GetCustomerGuestAlertByIdValidator : AbstractValidator<GetCustomerGuestAlertByIdRequest>
{
    public GetCustomerGuestAlertByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerGuestAlertByIdInValidator());

    }
    public class GetCustomerGuestAlertByIdInValidator : AbstractValidator<GetCustomerGuestAlertByIdIn>
    {
        public GetCustomerGuestAlertByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
