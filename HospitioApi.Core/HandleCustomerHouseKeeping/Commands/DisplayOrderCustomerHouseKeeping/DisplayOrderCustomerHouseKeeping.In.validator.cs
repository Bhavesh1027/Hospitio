using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Commands.DisplayOrderCustomerHouseKeeping
{
    public class DisplayOrderCustomerHouseKeepingValidator: AbstractValidator<DisplayOrderCustomerHouseKeepingRequest>
    {
        public DisplayOrderCustomerHouseKeepingValidator()
        {
            RuleFor(m => m.In).SetValidator(new DisplayOrderCustomerHouseKeepingInValidator());
        }

        public class DisplayOrderCustomerHouseKeepingInValidator : AbstractValidator<DisplayOrderCustomerHouseKeepingIn>
        {

        }
    }
}
