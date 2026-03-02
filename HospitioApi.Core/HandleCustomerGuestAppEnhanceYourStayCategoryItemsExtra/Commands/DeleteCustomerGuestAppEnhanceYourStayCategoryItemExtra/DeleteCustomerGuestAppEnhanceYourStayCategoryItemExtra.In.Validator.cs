using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtra;

public class DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraValidator : AbstractValidator<DeleteEnhanceYourStayCategoryItemExtraRequest>
{
    public DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomersGuestAppEnhanceYourStayCategoryItemExtraInValidator());
    }
    public class DeleteCustomersGuestAppEnhanceYourStayCategoryItemExtraInValidator : AbstractValidator<DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraIn>
    {
        public DeleteCustomersGuestAppEnhanceYourStayCategoryItemExtraInValidator()
        {
            RuleFor(m => m.CustomerGuestAppEnhanceYourStayItemId).NotEmpty().GreaterThan(0);
        }
    }
}
