using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyService.Queries.GetCustomerPropertyServiceById;

public class GetCustomerPropertyServiceByIdValidator : AbstractValidator<GetCustomerPropertyServiceByIdRequest>
{
    public GetCustomerPropertyServiceByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerPropertyServiceByIdInValidator());

    }
    public class GetCustomerPropertyServiceByIdInValidator : AbstractValidator<GetCustomerPropertyServiceByIdIn>
    {
        public GetCustomerPropertyServiceByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
