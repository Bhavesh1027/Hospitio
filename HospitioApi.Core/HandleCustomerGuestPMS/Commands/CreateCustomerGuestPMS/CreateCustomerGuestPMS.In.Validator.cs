using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestPMS.Commands.CreateCustomerGuestPMS;

public class CreateCustomerGuestPMSValidator : AbstractValidator<CreateCustomerGuestPMSRequest>
{
    public CreateCustomerGuestPMSValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerGuestPMSInValidator());
    }
    public class CreateCustomerGuestPMSInValidator : AbstractValidator<CreateCustomerGuestPMSIn>
    {
        public CreateCustomerGuestPMSInValidator()
        {
            RuleFor(m => m.DocumentAttachment).NotEmpty();
            //RuleFor(m => m.ContainerName).NotEmpty();
        }
    }
}
