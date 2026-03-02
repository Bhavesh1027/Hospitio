using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerAppBuilderById;

public class GetCustomerGuestAppBuilderByIdValidator : AbstractValidator<GetCustomerGuestAppBuilderByIdRequest>
{
    public GetCustomerGuestAppBuilderByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerGuestAppBuilderByIdInValidator());

    }
    public class GetCustomerGuestAppBuilderByIdInValidator : AbstractValidator<GetCustomerGuestAppBuilderByIdIn>
    {
        public GetCustomerGuestAppBuilderByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
