using FluentValidation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Queries.GetCustomerEnhanceYourStayByCategory;

public class GetCustomerEnhanceYourStayByCategoryValidator: AbstractValidator<GetCustomerEnhanceYourStayByCategoryRequest>
{
    public GetCustomerEnhanceYourStayByCategoryValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerEnhanceYourStayByCategoryInValidator());

    }
    public class GetCustomerEnhanceYourStayByCategoryInValidator : AbstractValidator<GetCustomerEnhanceYourStayByCategoryIn>
    {
        public GetCustomerEnhanceYourStayByCategoryInValidator()
        {
            RuleFor(m => m.CategoryId).NotEmpty().GreaterThan(0);
        }
    }
}
