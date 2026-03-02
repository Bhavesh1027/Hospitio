using FluentValidation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategories;

public class GetCustomerEnhanceYourStayCategoriesValidator : AbstractValidator<GetCustomerEnhanceYourStayCategoriesRequest>
{
    public GetCustomerEnhanceYourStayCategoriesValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerEnhanceYourStayCategoriesInValidator());

    }
    public class GetCustomerEnhanceYourStayCategoriesInValidator : AbstractValidator<GetCustomerEnhanceYourStayCategoriesIn>
    {
        public GetCustomerEnhanceYourStayCategoriesInValidator()
        {
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
            RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
        }
    }
}
