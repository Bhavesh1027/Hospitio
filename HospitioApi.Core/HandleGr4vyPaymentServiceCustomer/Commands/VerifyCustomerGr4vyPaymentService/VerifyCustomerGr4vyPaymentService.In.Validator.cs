using FluentValidation;

namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Commands.VerifyCustomerGr4vyPaymentService;

public class VerifyCustomerGr4vyPaymentServiceValidator : AbstractValidator<VerifyCustomerGr4vyPaymentServiceRequest>
{
    public VerifyCustomerGr4vyPaymentServiceValidator()
    {
        RuleFor(m => m.In).SetValidator(new VerifyCustomerGr4vyPaymentServiceInValidator());
    }
    public class VerifyCustomerGr4vyPaymentServiceInValidator : AbstractValidator<VerifyCustomerGr4vyPaymentServiceIn>
    {
        public VerifyCustomerGr4vyPaymentServiceInValidator()
        {
            RuleFor(m => m.PaymentProcessorId).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(m => m.Fields).NotNull();
        }
    }
}
