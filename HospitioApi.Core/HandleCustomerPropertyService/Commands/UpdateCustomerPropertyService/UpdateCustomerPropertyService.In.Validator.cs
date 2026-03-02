using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyService.Commands.UpdateCustomerPropertyService;

public class UpdateCustomerPropertyServiceValidator : AbstractValidator<UpdateCustomerPropertyServiceRequest>
{
    public UpdateCustomerPropertyServiceValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerPropertyServiceInValidator());
    }
    public class UpdateCustomerPropertyServiceInValidator : AbstractValidator<UpdateCustomerPropertyServiceIn>
    {
        public UpdateCustomerPropertyServiceInValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
    