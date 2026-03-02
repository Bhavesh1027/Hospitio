using FluentValidation;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.CreateCustomersPropertiesInfo;

public class CreateCustomersPropertiesInfoValidator : AbstractValidator<CreateCustomersPropertiesInfoRequest>
{
    public CreateCustomersPropertiesInfoValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomersPropertiesInfoInValidator());
    }
    public class CreateCustomersPropertiesInfoInValidator : AbstractValidator<CreateCustomersPropertiesInfoIn>
    {
        public CreateCustomersPropertiesInfoInValidator()
        {
            RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
            RuleFor(m => m.WifiUsername).NotEmpty();
            RuleFor(m => m.WifiPassword).NotEmpty();
            RuleFor(m => m.Overview).NotEmpty();
            RuleFor(m => m.CheckInPolicy).NotEmpty();
            RuleFor(m => m.TermsAndConditions).NotEmpty();
            RuleFor(m => m.Street).NotEmpty();
            RuleFor(m => m.StreetNumber).NotEmpty();
            RuleFor(m => m.City).NotEmpty();
            RuleFor(m => m.Postalcode).NotEmpty();
            RuleFor(m => m.Country).NotEmpty();
            RuleFor(m => m.IsActive).NotEmpty();
        }
    }
}
