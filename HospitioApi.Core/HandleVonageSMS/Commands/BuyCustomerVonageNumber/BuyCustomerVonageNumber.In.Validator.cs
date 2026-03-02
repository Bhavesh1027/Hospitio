using FluentValidation;

namespace HospitioApi.Core.HandleVonageSMS.Commands.BuyCustomerVonageNumber
{
    public class BuyCustomerVonageNumberValidator : AbstractValidator<BuyCustomerVonageNumberHandlerRequest>
    {
        public BuyCustomerVonageNumberValidator()
        {
            RuleFor(m => m.In).SetValidator(new CustomerVonageNumberValidator());
        }
        public class CustomerVonageNumberValidator : AbstractValidator<BuyCustomerVonageNumberIn>
        {
            public CustomerVonageNumberValidator()
            {
            }
        }

    }
}
