using FluentValidation;
using HospitioApi.Core.HandleVonageSMS.Queries.GetAvailableNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleVonageSMS.Commands.UpdateCustomerVonageNumber
{
    public class UpdateCustomerVonageNumberValidator:AbstractValidator<UpdateCustomerVonageNumberHandlerRequest>
    {
        public UpdateCustomerVonageNumberValidator()
        {
            RuleFor(m => m.In).SetValidator(new UpdatedCustomerVonageNumberValidator());
        }
        public class UpdatedCustomerVonageNumberValidator : AbstractValidator<UpdateCustomerVonageNumberIn>
        {
            public UpdatedCustomerVonageNumberValidator()
            {
            }
        }
    }
}
