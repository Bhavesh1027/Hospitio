using FluentValidation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategoryById;

public class GetCustomerEnhanceYourStayCategoryByIdValidator : AbstractValidator<GetCustomerEnhanceYourStayCategoryByIdRequest>
{
    public GetCustomerEnhanceYourStayCategoryByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerEnhanceYourStayCategoryByIdInValidator());
    }
    public class GetCustomerEnhanceYourStayCategoryByIdInValidator : AbstractValidator<GetCustomerEnhanceYourStayCategoryByIdIn>
    {
        public GetCustomerEnhanceYourStayCategoryByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
