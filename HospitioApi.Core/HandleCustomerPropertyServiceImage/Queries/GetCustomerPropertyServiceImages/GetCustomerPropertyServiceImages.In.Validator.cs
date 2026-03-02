using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyServiceImage.Queries.GetCustomerPropertyServiceImages;

public class GetCustomerPropertyServiceImagesValidator : AbstractValidator<GetCustomerPropertyServiceImagesRequest>
{
    public GetCustomerPropertyServiceImagesValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerPropertyServiceImagesInValidator());
    }
    public class GetCustomerPropertyServiceImagesInValidator : AbstractValidator<GetCustomerPropertyServiceImagesIn>
    {
        public GetCustomerPropertyServiceImagesInValidator()
        {
            RuleFor(m => m.CustomerPropertyServiceId).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
