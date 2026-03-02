using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyService.Commands.CreateCustomerPropertyService;

public class CreateCustomerPropertyServiceValidator : AbstractValidator<CreateCustomerPropertyServiceRequest>
{
    public CreateCustomerPropertyServiceValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerPropertyServiceInValidator());
    }
    public class CreateCustomerPropertyServiceInValidator : AbstractValidator<CreateCustomerPropertyServiceIn>
    {
        public CreateCustomerPropertyServiceInValidator()
        {
            RuleFor(m => m.CustomerPropertyInformationId).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(m => m.Name).NotEmpty().NotNull();
            RuleFor(m => m.IsActive).NotEmpty().NotNull().Must(x => x == false || x == true);
        }
    }
}
