using FluentValidation;
using HospitioApi.Core.HandleCustomersConcierge.Commands.DisplayOrderCustomerConcierage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomerReception.Commands.DisplayCustomerReception;

public class DisplayOrderCustomerReceptionValidator : AbstractValidator<DisplayOrderCustomerReceptionRequest>
{
    public DisplayOrderCustomerReceptionValidator()
    {
        RuleFor(m => m.In).SetValidator(new DisplayOrderCustomerReceptionInValidator());
    }

    public class DisplayOrderCustomerReceptionInValidator : AbstractValidator<DisplayOrderCustomerReceptionIn>
    {

    }
}
