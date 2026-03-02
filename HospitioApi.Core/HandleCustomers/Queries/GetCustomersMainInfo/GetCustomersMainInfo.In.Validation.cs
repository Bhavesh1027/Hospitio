using FluentValidation;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomersMainInfo;

public class GetCustomersMainInfoValidator : AbstractValidator<GetCustomersMainInfoRequest>
{
    public GetCustomersMainInfoValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerMainInfoValidator());
    }

    public class GetCustomerMainInfoValidator : AbstractValidator<GetCustomersMainInfoIn>
    {
        public GetCustomerMainInfoValidator()
        {
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
        }
    }
}
