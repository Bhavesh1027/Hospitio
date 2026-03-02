using FluentValidation;

namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Queries.GetCustomerGr4vyPaymentServiceById;

public class GetCustomerGr4vyPaymentServiceByIdValidator : AbstractValidator<GetCustomerGr4vyPaymentServiceByIdHandlerRequest>
{
    public GetCustomerGr4vyPaymentServiceByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerPaymentServiceInValidator());
    }

    public class GetCustomerPaymentServiceInValidator : AbstractValidator<GetCustomerGr4vyPaymentServiceByIdIn>
    {
        public GetCustomerPaymentServiceInValidator()
        {
            RuleFor(m => m.CustomerPaymentProcessorId).NotEmpty().GreaterThan(0);
        }
    }
}
