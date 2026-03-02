using FluentValidation;

namespace HospitioApi.Core.HandleCustomersConcierge.Commands.DeleteCustomerConciergeItem;

public class DeleteCustomerConciergeItemValidator : AbstractValidator<DeleteCustomerConciergeItemRequest>
{
    public DeleteCustomerConciergeItemValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerConciergeItemInValidator());
    }
    public class DeleteCustomerConciergeItemInValidator : AbstractValidator<DeleteCustomerConciergeItemIn>
    {
        public DeleteCustomerConciergeItemInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
