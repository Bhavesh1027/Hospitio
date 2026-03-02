using FluentValidation;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.DeleteCustomersPaymentProcessors;

public class DeleteCustomersPaymentProcessorsValidator : AbstractValidator<DeleteCustomersPaymentProcessorsRequest>
{
    public DeleteCustomersPaymentProcessorsValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomersPaymentProcessorsInValidator());
    }
    public class DeleteCustomersPaymentProcessorsInValidator : AbstractValidator<DeleteCustomersPaymentProcessorsIn>
    {
        public DeleteCustomersPaymentProcessorsInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
