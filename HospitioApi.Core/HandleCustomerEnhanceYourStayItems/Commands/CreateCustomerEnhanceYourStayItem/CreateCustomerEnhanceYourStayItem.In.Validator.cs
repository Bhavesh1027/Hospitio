using FluentValidation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.CreateCustomerEnhanceYourStayItem;

public class CreateCustomerEnhanceYourStayItemValidator : AbstractValidator<CreateCustomerEnhanceYourStayItemRequest>
{
    public CreateCustomerEnhanceYourStayItemValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerEnhanceYourStayItemInValidator());
    }
    public class CreateCustomerEnhanceYourStayItemInValidator : AbstractValidator<CreateCustomerEnhanceYourStayItemIn>
    {
        public CreateCustomerEnhanceYourStayItemInValidator()
        {
            //RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
            RuleFor(m => m.CustomerGuestAppBuilderCategoryId).NotEmpty();
            RuleFor(m => m.ButtonText).MaximumLength(12).WithMessage("Currency should not exceed 12 characters.");
            RuleFor(m => m.Currency).MaximumLength(3).WithMessage("Currency should not exceed 3 characters.");
            //RuleFor(m => m.DisplayOrder).NotEmpty();
        }
    }
}
