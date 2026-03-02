using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.UpdateCustomerPropertyEmergencyNumber;

public class UpdateCustomerPropertyEmergencyNumberValidator : AbstractValidator<UpdateCustomerPropertyEmergencyNumberRequest>
{
    public UpdateCustomerPropertyEmergencyNumberValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdatedCustomerPropertyEmergencyNumberValidator());
    }

    public class UpdatedCustomerPropertyEmergencyNumberValidator : AbstractValidator<UpdateCustomerPropertyEmergencyNumberIn>
    {
        public UpdatedCustomerPropertyEmergencyNumberValidator()
        {           
            //RuleFor(m => m.CustomerPropertyInformationId).NotEmpty().GreaterThan(0);
        }
    }
}
