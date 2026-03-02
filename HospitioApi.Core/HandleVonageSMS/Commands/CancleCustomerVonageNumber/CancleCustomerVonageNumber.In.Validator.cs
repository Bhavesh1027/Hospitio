using FluentValidation;

namespace HospitioApi.Core.HandleVonageSMS.Commands.CancleCustomerVonageNumber
{
    public class CancleCustomerVonageNumberValidator : AbstractValidator<CancleCustomerVonageNumberHandlerRequest>
    {
        public CancleCustomerVonageNumberValidator()
        {
            RuleFor(m => m.In).SetValidator(new CustomerVonageNumberValidator());
        }
        public class CustomerVonageNumberValidator : AbstractValidator<CancleCustomerVonageNumberIn>
        {
            public CustomerVonageNumberValidator()
            {
            }
        }
    }
}
