using FluentValidation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.UpdateCustomerEnhanceYourStayCategory;

public class UpdateCustomerEnhanceYourStayCategoryValidator : AbstractValidator<UpdateCustomerEnhanceYourStayCategoryRequest>
{
    public UpdateCustomerEnhanceYourStayCategoryValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerEnhanceYourStayCategoryInValidator());
    }

    public class UpdateCustomerEnhanceYourStayCategoryInValidator : AbstractValidator<UpdateCustomerEnhanceYourStayCategoryIn>
    {
        public UpdateCustomerEnhanceYourStayCategoryInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.CustomerId).NotEmpty();
            RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
            RuleFor(m => m.CategoryName).NotEmpty();
        }
    }
}
