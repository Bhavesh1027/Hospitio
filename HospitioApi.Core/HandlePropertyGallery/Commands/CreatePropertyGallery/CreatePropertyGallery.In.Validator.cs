using FluentValidation;

namespace HospitioApi.Core.HandlePropertyGallery.Commands.CreatePropertyGallery;

public class CreatePropertyGalleryValidator : AbstractValidator<CreatePropertyGalleryRequest>
{
    public CreatePropertyGalleryValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreatePropertyGalleryInValidator());
    }

    public class CreatePropertyGalleryInValidator : AbstractValidator<CreatePropertyGalleryIn>
    {
        public CreatePropertyGalleryInValidator()
        {
            //RuleFor(m => m.CustomerPropertyInformationId).NotNull().GreaterThan(0);
            //RuleFor(m => m.PropertyImage).NotEmpty();

        }
    }
}