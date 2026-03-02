using FluentValidation;

namespace HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Queries.GetGuestAppEnhanceYourStayItemImages;

public class GetGuestAppEnhanceYourStayItemImagesValidator : AbstractValidator<GetCustomersGuestAppEnhanceYourStayItemImagesRequest>
{
    public GetGuestAppEnhanceYourStayItemImagesValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetGuestAppEnhanceYourStayItemImagesInValidator());
    }

    public class GetGuestAppEnhanceYourStayItemImagesInValidator : AbstractValidator<GetGuestAppEnhanceYourStayItemImagesIn>
    {
        public GetGuestAppEnhanceYourStayItemImagesInValidator()
        {
            RuleFor(m => m.CustomerGuestAppEnhanceYourStayItemId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
        }
    }
}
