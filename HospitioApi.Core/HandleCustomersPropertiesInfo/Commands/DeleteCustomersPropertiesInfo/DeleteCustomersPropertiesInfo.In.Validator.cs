using FluentValidation;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.DeleteCustomersPropertiesInfo;

public class DeleteCustomersPropertiesInfoValidator : AbstractValidator<DeleteCustomersPropertiesInfoRequest>
{
    public DeleteCustomersPropertiesInfoValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomersPropertiesInfoInValidator());
    }
    public class DeleteCustomersPropertiesInfoInValidator : AbstractValidator<DeleteCustomersPropertiesInfoIn>
    {
        public DeleteCustomersPropertiesInfoInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
