using FluentValidation;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.DisplayOrderCustomerHouseKeeping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.DisplayOrderCustomerEnhanceYourStay
{
    public class DisplayOrderCustomerEnhanceYourStayValidator : AbstractValidator<DisplayOrderCustomerEnhanceYourStayRequest>
    {
        public DisplayOrderCustomerEnhanceYourStayValidator()
        {
            RuleFor(m => m.In).SetValidator(new DisplayOrderCustomerEnhanceYourStayInValidator());

        }

    }
    public class DisplayOrderCustomerEnhanceYourStayInValidator : AbstractValidator<DisplayOrderCustomerEnhanceYourStayIn>
    {

    }
}
