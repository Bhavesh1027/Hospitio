using FluentValidation;

namespace HospitioApi.Core.HandleCustomersConcierge.Commands.DeleteCustomerConcierge;

public class DeleteCustomerConciergeValidator : AbstractValidator<DeleteCustomerConciergeRequest>
{
    public DeleteCustomerConciergeValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerConciergeInValidator());
    }
    public class DeleteCustomerConciergeInValidator : AbstractValidator<DeleteCustomerConciergeIn>
    {
        public DeleteCustomerConciergeInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
