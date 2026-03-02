using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.CreateCustomerPropertyExtras;

public class CreateCustomerPropertyExtrasValidator : AbstractValidator<CreateCustomerPropertyExtrasRequest>
{
    public CreateCustomerPropertyExtrasValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerPropertyExtrasInValidator());
    }

    public class CreateCustomerPropertyExtrasInValidator : AbstractValidator<CreateCustomerPropertyExtrasIn>
    {
        public CreateCustomerPropertyExtrasInValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.CustomerPropertyInformationId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.Description).NotEmpty();
            RuleFor(m => m.Link).NotEmpty();
            RuleFor(m => m.ExtraType).NotEmpty();
        }
    }
}
