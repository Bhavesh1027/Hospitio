using FluentValidation;

namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Queries.GetHospitioPaymentProcessors;

public class GetHospitioPaymentProcessorsValidator : AbstractValidator<GetHospitioPaymentProcessorsRequest>
{
    public GetHospitioPaymentProcessorsValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetHospitioPaymentProcessorsInValidator());
    }
    public class GetHospitioPaymentProcessorsInValidator : AbstractValidator<GetHospitioPaymentProcessorsIn>
    {
        public GetHospitioPaymentProcessorsInValidator()
        {
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
        }
    }
}
