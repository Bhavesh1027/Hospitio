using FluentValidation;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomerByIdForHospitio;

public class GetCustomerByIdForHospitioValidator : AbstractValidator<GetCustomerByIdForHospitioRequest>
{
    public GetCustomerByIdForHospitioValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerByIdForHospitioInValidator());
    }

    public class GetCustomerByIdForHospitioInValidator : AbstractValidator<GetCustomerByIdForHospitioIn>
    {
        public GetCustomerByIdForHospitioInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
