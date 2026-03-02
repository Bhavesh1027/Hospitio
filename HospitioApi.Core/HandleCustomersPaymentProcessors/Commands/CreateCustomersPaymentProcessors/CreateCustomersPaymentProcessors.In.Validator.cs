using FluentValidation;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.CreateCustomersPaymentProcessors;

public class CreateCustomersPaymentProcessorsValidator : AbstractValidator<CreateCustomersPaymentProcessorsRequest>
{
    public CreateCustomersPaymentProcessorsValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomersPaymentProcessorsInValidator());
    }
    public class CreateCustomersPaymentProcessorsInValidator : AbstractValidator<CreateCustomersPaymentProcessorsIn>
    {
        public CreateCustomersPaymentProcessorsInValidator()
        {
            RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PaymentProcessorId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.ClientId).NotEmpty();
            RuleFor(m => m.ClientSecret).NotEmpty();
            RuleFor(m => m.Currency).NotEmpty();
        }
    }
}
