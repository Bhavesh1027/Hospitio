using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Queries.GetCustomerPropertyEmergencyNumberById;

public class GetCustomerPropertyEmergencyNumberByIdValidator : AbstractValidator<GetCustomerPropertyEmergencyNumberByIdRequest>
{
    public GetCustomerPropertyEmergencyNumberByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerPropertyEmergencyNumberByIdInValidator());
    }

    public class GetCustomerPropertyEmergencyNumberByIdInValidator : AbstractValidator<GetCustomerPropertyEmergencyNumberByIdIn>
    {
        public GetCustomerPropertyEmergencyNumberByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
