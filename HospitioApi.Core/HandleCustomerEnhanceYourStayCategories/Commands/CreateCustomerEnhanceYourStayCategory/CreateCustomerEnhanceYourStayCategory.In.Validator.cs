using FluentValidation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.CreateCustomerEnhanceYourStayCategory;

public class CreateCustomerEnhanceYourStayCategoryValidator : AbstractValidator<CreateCustomerEnhanceYourStayCategoryRequest>
{
    public CreateCustomerEnhanceYourStayCategoryValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerEnhanceYourStayCategoryInValidator());
    }
    public class CreateCustomerEnhanceYourStayCategoryInValidator : AbstractValidator<CreateCustomerEnhanceYourStayCategoryIn>
    {
        public CreateCustomerEnhanceYourStayCategoryInValidator()
        {
            RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
            RuleFor(m => m.CategoryName).NotEmpty();
        }
    }
}
