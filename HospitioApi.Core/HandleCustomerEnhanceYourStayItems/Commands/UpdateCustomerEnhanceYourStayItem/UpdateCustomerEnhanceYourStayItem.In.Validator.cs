using FluentValidation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.UpdateCustomerEnhanceYourStayItem;

public class UpdateCustomerEnhanceYourStayItemValidator : AbstractValidator<UpdateCustomerEnhanceYourStayItemRequest>
{
    public UpdateCustomerEnhanceYourStayItemValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerEnhanceYourStayItemInValidator());
    }

    public class UpdateCustomerEnhanceYourStayItemInValidator : AbstractValidator<UpdateCustomerEnhanceYourStayItemIn>
    {
        public UpdateCustomerEnhanceYourStayItemInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            //RuleFor(m => m.CustomerId).NotEmpty();
            RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
            RuleFor(m => m.CustomerGuestAppBuilderCategoryId).NotEmpty();
            RuleFor(m => m.ButtonText).MaximumLength(12).WithMessage("Currency should not exceed 12 characters.");
            RuleFor(m => m.Currency).MaximumLength(3).WithMessage("Currency should not exceed 3 characters.");
        }
    }
}
