using FluentValidation;

namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Commands.CreateHospitioPaymentProcessors;

public class CreateHospitioPaymentProcessorsValidator : AbstractValidator<CreateHospitioPaymentProcessorsRequest>
{
    public CreateHospitioPaymentProcessorsValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateHospitioPaymentProcessorsInValidator());
    }
    public class CreateHospitioPaymentProcessorsInValidator : AbstractValidator<CreateHospitioPaymentProcessorsIn>
    {
        public CreateHospitioPaymentProcessorsInValidator()
        {
            RuleFor(m => m.PaymentProcessorId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.ClientId).NotEmpty();
            RuleFor(m => m.ClientSecret).NotEmpty();
            RuleFor(m => m.Currency).NotEmpty();
        }
    }
}
