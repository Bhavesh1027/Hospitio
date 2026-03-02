using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyServiceImage.Commands.DeleteCustomerPropertyServiceImage;

public class DeleteCustomerPropertyServiceImageValidator : AbstractValidator<DeleteCustomerPropertyServiceImageRequest>
{
    public DeleteCustomerPropertyServiceImageValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerPropertyServiceImageInValidator());
    }
    public class DeleteCustomerPropertyServiceImageInValidator : AbstractValidator<DeleteCustomerPropertyServiceImageIn>
    {
        public DeleteCustomerPropertyServiceImageInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
