using FluentValidation;

namespace HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessorsById;

public class GetPaymentProcessorsByIdValidator : AbstractValidator<GetPaymentProcessorsByIdRequest>
{
    public GetPaymentProcessorsByIdValidator()
    {
        RuleFor(e => e.In).SetValidator(new GetPaymentProcessorsByIdInValidator());
    }
    public class GetPaymentProcessorsByIdInValidator : AbstractValidator<GetPaymentProcessorsByIdIn>
    {
        public GetPaymentProcessorsByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
