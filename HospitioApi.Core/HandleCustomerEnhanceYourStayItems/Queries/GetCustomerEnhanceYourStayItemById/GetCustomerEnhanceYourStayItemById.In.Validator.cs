using FluentValidation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Queries.GetCustomerEnhanceYourStayItemById;

public class GetCustomerEnhanceYourStayItemByIdValidator : AbstractValidator<GetCustomerEnhanceYourStayItemByIdRequest>
{
    public GetCustomerEnhanceYourStayItemByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerEnhanceYourStayItemByIdInValidator());
    }
    public class GetCustomerEnhanceYourStayItemByIdInValidator : AbstractValidator<GetCustomerEnhanceYourStayItemByIdIn>
    {
        public GetCustomerEnhanceYourStayItemByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
