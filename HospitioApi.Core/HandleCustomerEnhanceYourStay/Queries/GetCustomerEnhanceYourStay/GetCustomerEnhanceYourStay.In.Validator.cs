using FluentValidation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Queries.GetCustomerEnhanceYourStay;

public class GetCustomerEnhanceYourStayValidator : AbstractValidator<GetCustomerEnhanceYourStayRequest>
{
    public GetCustomerEnhanceYourStayValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerEnhanceYourStayInValidator());

    }
    public class GetCustomerEnhanceYourStayInValidator : AbstractValidator<GetCustomerEnhanceYourStayIn>
    {
        public GetCustomerEnhanceYourStayInValidator()
        {
            RuleFor(m => m.BuilderId).NotEmpty().GreaterThan(0);
        }
    }
}
