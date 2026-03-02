using FluentValidation;

namespace HospitioApi.Core.HandleCustomersConcierge.Commands.DisplayOrderCustomerConcierage;

public class DisplayOrderCustomerConcierageValidator : AbstractValidator<DisplayOrderCustomerConcierageRequest>
{
    public DisplayOrderCustomerConcierageValidator()
    {
        RuleFor(m => m.In).SetValidator(new DisplayOrderCustomerConcierageInValidator());
    }

    public class DisplayOrderCustomerConcierageInValidator : AbstractValidator<DisplayOrderCustomerConcierageIn>
    {
       
    }
}