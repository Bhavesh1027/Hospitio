using FluentValidation;

namespace HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Commands.CreateGuestAppEnhanceYourStayItemsImages;

public class CreateGuestAppEnhanceYourStayItemImageValidator : AbstractValidator<CreateGuestAppEnhanceYourStayItemImageRequest>
{
    public CreateGuestAppEnhanceYourStayItemImageValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateGuestAppEnhanceYourStayItemImageInValidator());
    }
    public class CreateGuestAppEnhanceYourStayItemImageInValidator : AbstractValidator<CreateGuestAppEnhanceYourStayItemImageIn>
    {
        public CreateGuestAppEnhanceYourStayItemImageInValidator()
        {
            RuleFor(m => m.CustomerGuestAppEnhanceYourStayItemId).NotEmpty().GreaterThan(0);
        }
    }
}
