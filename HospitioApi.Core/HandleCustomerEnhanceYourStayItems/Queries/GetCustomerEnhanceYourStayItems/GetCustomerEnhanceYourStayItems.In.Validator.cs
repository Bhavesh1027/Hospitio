using FluentValidation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Queries.GetCustomerEnhanceYourStayItems;

public class GetCustomerEnhanceYourStayItemsValidator : AbstractValidator<GetCustomerEnhanceYourStayItemsRequest>
{
    public GetCustomerEnhanceYourStayItemsValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerEnhanceYourStayItemsInValidator());

    }
    public class GetCustomerEnhanceYourStayItemsInValidator : AbstractValidator<GetCustomerEnhanceYourStayItemsIn>
    {
        public GetCustomerEnhanceYourStayItemsInValidator()
        {
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
            RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
        }
    }
}
