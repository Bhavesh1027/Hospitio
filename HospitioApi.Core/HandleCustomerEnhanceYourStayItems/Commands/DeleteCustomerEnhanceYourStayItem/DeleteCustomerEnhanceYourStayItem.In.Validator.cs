using FluentValidation;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.DeleteCustomerEnhanceYourStayItem;

public class DeleteCustomerEnhanceYourStayItemValidator : AbstractValidator<DeleteCustomerEnhanceYourStayItemRequest>
{
    public DeleteCustomerEnhanceYourStayItemValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerEnhanceYourStayItemInValidator());
    }
    public class DeleteCustomerEnhanceYourStayItemInValidator : AbstractValidator<DeleteCustomerEnhanceYourStayItemIn>
    {
        public DeleteCustomerEnhanceYourStayItemInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
