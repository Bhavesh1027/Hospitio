using FluentValidation;
using HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategoryWithRelation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategoriesWithRelation;

public class GetCustomerEnhanceYourStayCategoriesWithRelationValidator : AbstractValidator<GetCustomerEnhanceYourStayCategoriesWithRelationRequest>
{
    public GetCustomerEnhanceYourStayCategoriesWithRelationValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerEnhanceYourStayCategoriesWithRelationInValidator());

    }
    public class GetCustomerEnhanceYourStayCategoriesWithRelationInValidator : AbstractValidator<GetCustomerEnhanceYourStayCategoriesWithRelationIn>
    {
        public GetCustomerEnhanceYourStayCategoriesWithRelationInValidator()
        {
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
            RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
        }
    }
}
