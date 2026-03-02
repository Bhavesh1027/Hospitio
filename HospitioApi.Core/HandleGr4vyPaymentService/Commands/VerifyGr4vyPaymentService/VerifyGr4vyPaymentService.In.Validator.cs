using FluentValidation;

namespace HospitioApi.Core.HandleGr4vyPaymentService.Commands.VerifyGr4vyPaymentService;

public class VerifyGr4vyPaymentServiceValidator : AbstractValidator<VerifyGr4vyPaymentServiceRequest>
{
    public VerifyGr4vyPaymentServiceValidator()
    {
        RuleFor(m => m.In).SetValidator(new VerifyGr4vyPaymentServiceInValidator());
    }
    public class VerifyGr4vyPaymentServiceInValidator : AbstractValidator<VerifyGr4vyPaymentServiceIn>
    {
        public VerifyGr4vyPaymentServiceInValidator()
        {
            RuleFor(m => m.PaymentProcessorId).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(m => m.Fields).NotNull();
        }
    }
}
