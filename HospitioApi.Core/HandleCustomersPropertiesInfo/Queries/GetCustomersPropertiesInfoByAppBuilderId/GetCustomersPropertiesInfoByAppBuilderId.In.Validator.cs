using FluentValidation;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoByAppBuilderId;

public class GetCustomersPropertiesInfoByAppBuilderIdValidator : AbstractValidator<GetCustomersPropertiesInfoByAppBuilderIdRequest>
{
    public GetCustomersPropertiesInfoByAppBuilderIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomersPropertiesInfoByAppBuilderIdInValidator());
    }

    public class GetCustomersPropertiesInfoByAppBuilderIdInValidator : AbstractValidator<GetCustomersPropertiesInfoByAppBuilderIdIn>
    {
        public GetCustomersPropertiesInfoByAppBuilderIdInValidator()
        {
            RuleFor(m => m.AppBuilderId).NotEmpty().GreaterThan(0);
        }
    }
}
