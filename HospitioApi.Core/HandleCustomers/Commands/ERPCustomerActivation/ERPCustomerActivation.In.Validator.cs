using FluentValidation;

namespace HospitioApi.Core.HandleCustomers.Commands.ERPCustomerActivation
{
    public class ERPCustomerActivationValidator : AbstractValidator<ERPCustomerActivationRequest>
    {
        public ERPCustomerActivationValidator()
        {
            RuleFor(m => m.In).SetValidator(new ERPCustomerActivationInValidator());
        }
    }
    public class ERPCustomerActivationInValidator : AbstractValidator<ERPCustomerActivationIn>
    {
        public ERPCustomerActivationInValidator()
        {
            RuleFor(m => m.PylonUniqueCustomerId).NotNull().NotEmpty();
            RuleFor(m => m.CustomerStatus).NotNull();
        }
    }
}
