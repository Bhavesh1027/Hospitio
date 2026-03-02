using FluentValidation;

namespace HospitioApi.Core.HandlePaymentProcessors.Commands.DeletePaymentProcessors;

public class DeletePaymentProcessorsValidator : AbstractValidator<DeletePaymentProcessorsRequest>
{
    public DeletePaymentProcessorsValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeletePaymentProcessorsInValidator());
    }
    public class DeletePaymentProcessorsInValidator : AbstractValidator<DeletePaymentProcessorsIn>
    {
        public DeletePaymentProcessorsInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
