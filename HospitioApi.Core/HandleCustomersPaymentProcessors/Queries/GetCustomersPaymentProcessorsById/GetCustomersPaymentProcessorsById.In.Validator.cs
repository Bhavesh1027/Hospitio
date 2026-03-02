using FluentValidation;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessorsById;

public class GetCustomersPaymentProcessorsByIdValidator : AbstractValidator<GetCustomersPaymentProcessorsByIdRequest>
{
    public GetCustomersPaymentProcessorsByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomersPaymentProcessorsByIdInValidator());
    }
    public class GetCustomersPaymentProcessorsByIdInValidator : AbstractValidator<GetCustomersPaymentProcessorsByIdIn>
    {
        public GetCustomersPaymentProcessorsByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
