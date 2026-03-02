using FluentValidation;
using HospitioApi.Core.HandlePaymentProcessors.Commands.CreatePaymentProcessors;

namespace HospitioApi.Core.HandlePaymentProcessors.Commands.CreatePaymentProcessorsl;

public class CreatePaymentProcessorsValidator : AbstractValidator<CreatePaymentProcessorsRequest>
{
    public CreatePaymentProcessorsValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreatePaymentProcessorsInValidator());
    }
    public class CreatePaymentProcessorsInValidator : AbstractValidator<CreatePaymentProcessorsIn>
    {
        public CreatePaymentProcessorsInValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}
