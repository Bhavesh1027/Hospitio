using FluentValidation;
using HospitioApi.Core.HandleVonageSMS.Commands.BuyCustomerVonageNumber;
using HospitioApi.Core.HandleVonageSMS.Commands.GetAvailableNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleVonageSMS.Queries.GetAvailableNumbers
{
    public class GetAvailableNumbersValidator : AbstractValidator<GetAvailableNumbersHandlerRequest>
    {
        public GetAvailableNumbersValidator()
        {
            RuleFor(m => m.In).SetValidator(new AvailableNumbersValidator());
        }
        public class AvailableNumbersValidator : AbstractValidator<GetAvailableNumbersIn>
        {
            public AvailableNumbersValidator()
            {
            }
        }
    }
}
