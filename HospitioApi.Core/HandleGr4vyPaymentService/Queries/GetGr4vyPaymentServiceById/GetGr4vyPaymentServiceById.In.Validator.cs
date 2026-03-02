using FluentValidation;

namespace HospitioApi.Core.HandleGr4vyPaymentService.Queries.GetGr4vyPaymentServiceById;

public class GetGr4vyPaymentServiceByIdValidator : AbstractValidator<GetGr4vyPaymentServiceByIdHandlerRequest>
{
    public GetGr4vyPaymentServiceByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetPaymentServiceInValidator());
    }

    public class GetPaymentServiceInValidator : AbstractValidator<GetGr4vyPaymentServiceByIdIn>
    {
        public GetPaymentServiceInValidator()
        {
            RuleFor(m => m.HospitioPaymentProcessorId).NotEmpty().GreaterThan(0);
        }
    }
}
