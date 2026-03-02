using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.DeleteCustomerPropertyEmergencyNumber;

public class DeleteCustomerPropertyEmergencyNumberValidator : AbstractValidator<DeleteCustomerPropertyEmergencyNumberRequest>
{
    public DeleteCustomerPropertyEmergencyNumberValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerPropertyEmergencyNumberInValidator());
    }

    public class DeleteCustomerPropertyEmergencyNumberInValidator : AbstractValidator<DeleteCustomerPropertyEmergencyNumberIn>
    {
        public DeleteCustomerPropertyEmergencyNumberInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
