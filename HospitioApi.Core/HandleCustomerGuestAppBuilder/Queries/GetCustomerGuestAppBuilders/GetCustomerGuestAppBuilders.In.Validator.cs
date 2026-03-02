using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerAppBuilders;

public class GetCustomerGuestAppBuildersValidator : AbstractValidator<GetCustomerGuestAppBuildersRequest>
{
    public GetCustomerGuestAppBuildersValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerGuestAppBuildersInValidator());

    }
    public class GetCustomerGuestAppBuildersInValidator : AbstractValidator<GetCustomerGuestAppBuildersIn>
    {
        public GetCustomerGuestAppBuildersInValidator()
        {
            RuleFor(m => m.CustomerId).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
        }
    }
}
