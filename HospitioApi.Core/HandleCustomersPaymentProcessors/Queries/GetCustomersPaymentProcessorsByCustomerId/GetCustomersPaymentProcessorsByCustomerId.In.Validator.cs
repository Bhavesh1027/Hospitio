using FluentValidation;
using HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessorsById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessorsUsingCustomerId;


public class GetCustomersPaymentProcessorsByCustomerIdValidator: AbstractValidator<GetCustomersPaymentProcessorsByCustomerIdRequest>
{

    public GetCustomersPaymentProcessorsByCustomerIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomersPaymentProcessorsByCustomerIdInValidator());
    }
    public class GetCustomersPaymentProcessorsByCustomerIdInValidator : AbstractValidator<GetCustomersPaymentProcessorsByCustomerIdIn>
    {
        public GetCustomersPaymentProcessorsByCustomerIdInValidator()
        {
            RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
        }
    }
}

