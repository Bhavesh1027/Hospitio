using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.CreateCustomerGuestAppEnhanceYourStayCategoryItemExtra;

public class CustomerGuestAppEnhanceYourStayCategoryItemExtraValidator : AbstractValidator<CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraRequest>
{
    public CustomerGuestAppEnhanceYourStayCategoryItemExtraValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateGuestAppEnhanceYourStayCategoryItemExtraInValidator());
    }
    public class CreateGuestAppEnhanceYourStayCategoryItemExtraInValidator : AbstractValidator<CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraIn>
    {
        public CreateGuestAppEnhanceYourStayCategoryItemExtraInValidator()
        {
            RuleFor(m => m.CustomerGuestAppEnhanceYourStayItemId).NotEmpty().GreaterThan(0);
        }
    }
}
