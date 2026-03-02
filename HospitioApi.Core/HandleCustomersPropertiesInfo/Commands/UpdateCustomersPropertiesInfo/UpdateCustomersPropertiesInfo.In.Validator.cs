using FluentValidation;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.UpdateCustomersPropertiesInfo;

public class UpdateCustomersPropertiesInfoValidator : AbstractValidator<UpdateCustomersPropertiesInfoRequest>
{
    public UpdateCustomersPropertiesInfoValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomersProprtiesInfoInValidator());
        RuleFor(m => m.CustomerId).NotNull().NotEmpty();
    }

    public class UpdateCustomersProprtiesInfoInValidator : AbstractValidator<UpdateCustomersPropertiesInfoIn>
    {
        public UpdateCustomersProprtiesInfoInValidator()
        {
            //RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            //RuleFor(m => m.CustomerId).NotEmpty();
            RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
            //RuleFor(m => m.WifiUsername).NotEmpty();
            //RuleFor(m => m.WifiPassword).NotEmpty();
            //RuleFor(m => m.Overview).NotEmpty();
            //RuleFor(m => m.CheckInPolicy).NotEmpty();
            //RuleFor(m => m.TermsAndConditions).NotEmpty();
            //RuleFor(m => m.Street).NotEmpty();
            RuleFor(m => m.StreetNumber).MaximumLength(5);
            //RuleFor(m => m.City).NotEmpty();
            //RuleFor(m => m.Postalcode).NotEmpty();
            RuleFor(m => m.Country).MaximumLength(3);
        }
    }
}
