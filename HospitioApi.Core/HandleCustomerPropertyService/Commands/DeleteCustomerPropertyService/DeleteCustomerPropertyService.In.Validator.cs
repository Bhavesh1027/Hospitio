using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyService.Commands.DeleteCustomerPropertyService;

public class DeleteCustomerPropertyServiceValidator : AbstractValidator<DeleteCustomerPropertyServiceRequest>
{
    public DeleteCustomerPropertyServiceValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerPropertyServiceInValidator());
    }

    public class DeleteCustomerPropertyServiceInValidator : AbstractValidator<DeleteCustomerPropertyServiceIn>
    {
        public DeleteCustomerPropertyServiceInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
