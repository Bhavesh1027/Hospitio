using FluentValidation;
using HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestPortalCheckInFormBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestCheckInFormBuilderIcon
{
    public class GetCustomerGuestCheckInFormBuilderIconValidator : AbstractValidator<GetCustomerGuestPortalCheckInFormBuilderIconRequest>
    {
        public GetCustomerGuestCheckInFormBuilderIconValidator()
        {
            RuleFor(m => m.In).SetValidator(new GetCustomerGuestsCheckInFormBuilderIconInValidator());
        }

        public class GetCustomerGuestsCheckInFormBuilderIconInValidator : AbstractValidator<GetCustomerGuestCheckInFormBuilderIconIn>
        {
            public GetCustomerGuestsCheckInFormBuilderIconInValidator()
            {
            }
        }
    }
}
