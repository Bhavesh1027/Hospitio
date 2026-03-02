using FluentValidation;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfo;

public class GetCustomersPropertiesInfoValidator : AbstractValidator<GetCustomersPropertiesInfoRequest>
{
    public GetCustomersPropertiesInfoValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetPropertiesInfoInValidator());

    }
    public class GetPropertiesInfoInValidator : AbstractValidator<GetCustomersPropertiesInfoIn>
    {
        public GetPropertiesInfoInValidator()
        {
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
            RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
        }
    }
}
