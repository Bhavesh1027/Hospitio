using FluentValidation;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoById;

public class GetCustomersPropertiesInfoByIdValidator : AbstractValidator<GetCustomersPropertiesInfoByIdRequest>
{
    public GetCustomersPropertiesInfoByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomersPropertiesInfoByIdInValidator());
    }
    public class GetCustomersPropertiesInfoByIdInValidator : AbstractValidator<GetCustomersPropertiesInfoByIdIn>
    {
        public GetCustomersPropertiesInfoByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
