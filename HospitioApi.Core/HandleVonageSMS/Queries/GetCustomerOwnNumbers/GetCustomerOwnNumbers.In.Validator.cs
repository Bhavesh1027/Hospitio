using FluentValidation;

namespace HospitioApi.Core.HandleVonageSMS.Queries.GetCustomerOwnNumbers
{
    public class GetCustomerOwnNumbersValidator : AbstractValidator<GetCustomerOwnNumbersHandlerRequest>
    {
        public GetCustomerOwnNumbersValidator()
        {
            RuleFor(m => m.In).SetValidator(new CustomerOwnNumbersValidator());
        }
        public class CustomerOwnNumbersValidator : AbstractValidator<GetCustomerOwnNumbersIn>
        {
            public CustomerOwnNumbersValidator()
            {
            }
        }
    }
}
