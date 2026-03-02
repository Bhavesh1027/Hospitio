using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyService.Queries.GetCustomerPropertyServices;

public class GetCustomerPropertyServicesValidator : AbstractValidator<GetCustomerPropertyServicesRequest>
{
    public GetCustomerPropertyServicesValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerPropertyServicesInValidator());

    }
    public class GetCustomerPropertyServicesInValidator : AbstractValidator<GetCustomerPropertyServicesIn>
    {
        public GetCustomerPropertyServicesInValidator()
        {
            RuleFor(m => m.CustomerPropertyInformationId).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
