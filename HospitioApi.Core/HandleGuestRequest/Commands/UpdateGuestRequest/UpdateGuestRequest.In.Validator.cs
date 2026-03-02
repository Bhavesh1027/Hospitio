using FluentValidation;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGuestRequest.Commands.UpdateGuestRequest;

public class UpdateGuestRequestValidator : AbstractValidator<UpdateGuestRequestRequest>
{
    public UpdateGuestRequestValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateGuestRequestInValidator());
    }
    public class UpdateGuestRequestInValidator : AbstractValidator<UpdateGuestRequestIn>
    {
        public UpdateGuestRequestInValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
            RuleFor(m => m.Status).IsInEnum();
        }
    }
}
