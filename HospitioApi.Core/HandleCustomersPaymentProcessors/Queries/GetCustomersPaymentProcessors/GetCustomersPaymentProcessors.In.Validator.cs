using FluentValidation;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessors;

public class GetCustomersPaymentProcessorsValidator : AbstractValidator<GetCustomersPaymentProcessorsRequest>
{
    public GetCustomersPaymentProcessorsValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomersPaymentProcessorsInValidator());
    }
    public class GetCustomersPaymentProcessorsInValidator : AbstractValidator<GetCustomersPaymentProcessorsIn>
    {
        public GetCustomersPaymentProcessorsInValidator()
        {
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
        }
    }
}
