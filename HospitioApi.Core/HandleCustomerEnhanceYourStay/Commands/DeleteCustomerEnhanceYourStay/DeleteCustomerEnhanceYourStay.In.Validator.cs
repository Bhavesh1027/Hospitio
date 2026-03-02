using FluentValidation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.DeleteCustomerEnhanceYourStay;

public class DeleteCustomerEnhanceYourStayValidator: AbstractValidator<DeleteCustomerEnhanceYourStayRequest>
{
    public DeleteCustomerEnhanceYourStayValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerEnhanceYourStayInValidator());
    }
    public class DeleteCustomerEnhanceYourStayInValidator : AbstractValidator<DeleteCustomerEnhanceYourStayIn>
    {
        public DeleteCustomerEnhanceYourStayInValidator()
        {
            RuleFor(m => m.CategoryId).NotEmpty().GreaterThan(0);
        }
    }
}
