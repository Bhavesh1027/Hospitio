using FluentValidation;
using FluentValidation.Validators;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Queries.GetCustomerPropertyEmergencyNumbers;

public class GetCustomerPropertyEmergencyNumbersValidator :AbstractValidator<GetCustomerPropertyEmergencyNumbersRequest>
{
    public GetCustomerPropertyEmergencyNumbersValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerPropertyEmergencyNumbersInValidator());
    }
    public class GetCustomerPropertyEmergencyNumbersInValidator : AbstractValidator<GetCustomerPropertyEmergencyNumbersIn>
    {
        public GetCustomerPropertyEmergencyNumbersInValidator()
        {
            RuleFor(m => m.PropertyId).NotEmpty().GreaterThan(0);
        }
    }
}
