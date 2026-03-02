using FluentValidation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.DeleteCustomerEnhanceYourStayCategory;

public class DeleteCustomerEnhanceYourStayCategoryValidator : AbstractValidator<DeleteCustomerEnhanceYourStayCategoryRequest>
{
    public DeleteCustomerEnhanceYourStayCategoryValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerEnhanceYourStayCategoryInValidator());
    }
    public class DeleteCustomerEnhanceYourStayCategoryInValidator : AbstractValidator<DeleteCustomerEnhanceYourStayCategoryIn>
    {
        public DeleteCustomerEnhanceYourStayCategoryInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
