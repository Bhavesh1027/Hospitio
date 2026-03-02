using FluentValidation;


namespace HospitioApi.Core.HandlePropertyGallery.Commands.EditPropertyGallery;

public class EditPropertyGalleryValidator : AbstractValidator<EditPropertyGalleryRequest>
{
    public EditPropertyGalleryValidator()
    {
        RuleFor(m => m.In).SetValidator(new EditPropertyGalleryInValidator());
    }

    public class EditPropertyGalleryInValidator : AbstractValidator<EditPropertyGalleryIn>
    {
        public EditPropertyGalleryInValidator()
        {
            RuleFor(m => m.CustomerPropertyInformationId).NotNull().GreaterThan(0);
            RuleFor(m => m.PropertyImages).NotEmpty();
        }
    }
}