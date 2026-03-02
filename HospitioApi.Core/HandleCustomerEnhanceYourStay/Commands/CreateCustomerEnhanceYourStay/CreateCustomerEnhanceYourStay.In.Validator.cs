using FluentValidation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.CreateCustomerEnhanceYourStay;

public class CreateCustomerEnhanceYourStayValidator: AbstractValidator<CreateCustomerEnhanceYourStayRequest>
{
    public CreateCustomerEnhanceYourStayValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerEnhanceYourStayInValidator());
    }
    public class CreateCustomerEnhanceYourStayInValidator : AbstractValidator<CreateCustomerEnhanceYourStayIn>
    {
        public CreateCustomerEnhanceYourStayInValidator()
        {
            //RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
        }
    }
}
