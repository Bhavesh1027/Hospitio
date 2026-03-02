using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Queries.GetCustomerPropertyExtras;

public class GetCustomerPropertyExtrasValidator : AbstractValidator<GetCustomerPropertyExtrasRequest>
{
    public GetCustomerPropertyExtrasValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerPropertyExtrasInValidator());
    }

    public class GetCustomerPropertyExtrasInValidator : AbstractValidator<GetCustomerPropertyExtrasIn>
    {
        public GetCustomerPropertyExtrasInValidator()
        {
            RuleFor(m => m.CustomerPropertyInformationId).NotEmpty().GreaterThan(0);
        }
    }
}
