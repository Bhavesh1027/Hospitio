using FluentValidation;
using HospitioApi.Core.HandleCustomersConcierge.Commands.DisplayOrderCustomerConcierage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HospitioApi.Core.HandleCustomersConcierge.Commands.DisplayOrderCustomerConcierage.DisplayOrderCustomerConcierageValidator;

namespace HospitioApi.Core.HandleCustomerRoomService.Commands.DisplayOrderCustomerRoomService
{
    public class DisplayOrderCustomerRoomServiceValidator : AbstractValidator<DisplayOrderCustomerRoomServiceRequest>
    {
        public DisplayOrderCustomerRoomServiceValidator()
        {
            RuleFor(m => m.In).SetValidator(new DisplayOrderCustomerRoomServiceInValidator());
        }
        
        public class DisplayOrderCustomerRoomServiceInValidator : AbstractValidator<DisplayOrderCustomerRoomServiceIn>
        {

        }
    }
}
