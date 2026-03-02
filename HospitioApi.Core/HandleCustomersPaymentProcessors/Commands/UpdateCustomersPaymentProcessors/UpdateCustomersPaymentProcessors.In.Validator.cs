using FluentValidation;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.UpdateCustomersPaymentProcessors;

public class UpdateCustomersPaymentProcessorsValidator : AbstractValidator<UpdateCustomersPaymentProcessorsRequest>
{
    public UpdateCustomersPaymentProcessorsValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomersPaymentProcessorsInValidator());
    }

    public class UpdateCustomersPaymentProcessorsInValidator : AbstractValidator<UpdateCustomersPaymentProcessorsIn>
    {
        public UpdateCustomersPaymentProcessorsInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PaymentProcessorId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.ClientId).NotEmpty();
            RuleFor(m => m.ClientSecret).NotEmpty();
            RuleFor(m => m.Currency).NotEmpty();
        }
    }
}
