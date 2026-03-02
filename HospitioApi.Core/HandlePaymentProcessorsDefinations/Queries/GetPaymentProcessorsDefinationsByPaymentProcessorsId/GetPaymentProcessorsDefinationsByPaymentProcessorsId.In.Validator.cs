using FluentValidation;
using HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessorsById;
using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessorByPaymentProcessorsId

{

public class GetPaymentProcessorsDefinationsByPaymentProcessorsIdValidator : AbstractValidator<GetPaymentProcessorsDefinationsByPaymentProcessorsIdRequest>
{
    public GetPaymentProcessorsDefinationsByPaymentProcessorsIdValidator()
    {
        RuleFor(e => e.In).SetValidator(new GetPaymentProcessorsDefinationsByPaymentProcessorsInValidator());
    }
    public class GetPaymentProcessorsDefinationsByPaymentProcessorsInValidator : AbstractValidator<GetPaymentProcessorsDefinationsByPaymentProcessorsIdIn>
    {
        public GetPaymentProcessorsDefinationsByPaymentProcessorsInValidator()
        {
            RuleFor(m => m.PaymentProcessorId).NotEmpty().GreaterThan(0);
        }
    }
}

}



