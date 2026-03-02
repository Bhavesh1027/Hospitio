using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Queries.GetCustomerPropertyExtraById;

public class GetCustomerPropertyExtraByIdValidator : AbstractValidator<GetCustomerPropertyExtraByIdRequest>
{
    public GetCustomerPropertyExtraByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerPropertyExtraByIdInValidator());
    }

    public class GetCustomerPropertyExtraByIdInValidator : AbstractValidator<GetCustomerPropertyExtraByIdIn>
    {
        public GetCustomerPropertyExtraByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
