using FluentValidation;

namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Commands.UpdateHospitioPaymentProcessors;

public class UpdateHospitioPaymentProcessorsValidator : AbstractValidator<UpdateHospitioPaymentProcessorsRequest>
{
    public UpdateHospitioPaymentProcessorsValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdatedHospitioPaymentProcessorsValidator());
    }
    public class UpdatedHospitioPaymentProcessorsValidator : AbstractValidator<UpdateHospitioPaymentProcessorsIn>
    {
        public UpdatedHospitioPaymentProcessorsValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PaymentProcessorId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.ClientId).NotEmpty();
            RuleFor(m => m.ClientSecret).NotEmpty();
            RuleFor(m => m.Currency).NotEmpty();
        }
    }
}
