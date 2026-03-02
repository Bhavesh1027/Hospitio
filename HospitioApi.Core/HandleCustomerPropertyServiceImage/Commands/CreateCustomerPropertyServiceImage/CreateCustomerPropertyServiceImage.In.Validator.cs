using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyServiceImage.Commands.CreateCustomerPropertyServiceImage;

public class CreateCustomerPropertyServiceImageValidator : AbstractValidator<CreateCustomerPropertyServiceImageRequest>
{
    public CreateCustomerPropertyServiceImageValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerPropertyServiceImageInValidator());
    }
    public class CreateCustomerPropertyServiceImageInValidator : AbstractValidator<CreateCustomerPropertyServiceImageIn>
    {
        public CreateCustomerPropertyServiceImageInValidator()
        {
            RuleFor(m => m.CustomerPropertyServiceId).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(m => m.IsActive).NotEmpty().NotNull().Must(x => x == false || x == true);
        }
    }
}
