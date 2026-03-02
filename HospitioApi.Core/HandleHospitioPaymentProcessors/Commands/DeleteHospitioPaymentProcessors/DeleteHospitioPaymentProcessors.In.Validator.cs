using FluentValidation;

namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Commands.DeleteHospitioPaymentProcessors;

public class DeleteHospitioPaymentProcessorsValidator : AbstractValidator<DeleteHospitioPaymentProcessorsRequest>
{
    public DeleteHospitioPaymentProcessorsValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteHospitioPaymentProcessorsInValidator());
    }
    public class DeleteHospitioPaymentProcessorsInValidator : AbstractValidator<DeleteHospitioPaymentProcessorsIn>
    {
        public DeleteHospitioPaymentProcessorsInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
