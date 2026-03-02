using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.CreateCustomerPropertyEmergencyNumber;

public class CreateCustomerPropertyEmergencyNumberVallidator : AbstractValidator<CreateCustomerPropertyEmergencyNumberRequest>
{
    public CreateCustomerPropertyEmergencyNumberVallidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerPropertyEmergencyNumberInVallidator());
    }

    public class CreateCustomerPropertyEmergencyNumberInVallidator : AbstractValidator<CreateCustomerPropertyEmergencyNumberIn>
    {
        public CreateCustomerPropertyEmergencyNumberInVallidator()
        {
            RuleFor(m => m.CustomerPropertyInformationId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.PhoneNumber).NotEmpty();
            RuleFor(m => m.PhoneCountry).NotEmpty().MaximumLength(3);
        }
    }
}
