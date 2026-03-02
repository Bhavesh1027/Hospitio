using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.UpdateCustomerPropertyExtras;

public class UpdateCustomerPropertyExtrasValidator : AbstractValidator<UpdateCustomerPropertyExtrasRequest>
{
    public UpdateCustomerPropertyExtrasValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerPropertyExtrasInValidator());
    }

    public class UpdateCustomerPropertyExtrasInValidator : AbstractValidator<UpdateCustomerPropertyExtrasIn>
    {
        public UpdateCustomerPropertyExtrasInValidator()
        {
            //RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            //RuleFor(m => m.Name).NotEmpty();
            //RuleFor(m => m.CustomerPropertyInformationId).NotEmpty().GreaterThan(0);
            //RuleFor(m => m.Description).NotEmpty();
            //RuleFor(m => m.Link).NotEmpty();
            //RuleFor(m => m.ExtraType).NotEmpty();
        }
    }
}
