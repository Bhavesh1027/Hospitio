using FluentValidation;

namespace HospitioApi.Core.HandleHospitioOnBoarding.Commands.UpdateHospitioOnBoardings;

public class UpdateHospitioOnBoardingsValidator : AbstractValidator<UpdateHospitioOnBoardingsRequest>
{
    public UpdateHospitioOnBoardingsValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateHospitioOnBoardingsInValidator());
    }
    public class UpdateHospitioOnBoardingsInValidator : AbstractValidator<UpdateHospitioOnBoardingsIn>
    {
        public UpdateHospitioOnBoardingsInValidator()
        {
            RuleFor(m => m.WhatsappCountry).MaximumLength(3);
            RuleFor(m => m.ViberCountry).MaximumLength(3);
            RuleFor(m => m.TelegramCountry).MaximumLength(3);
            RuleFor(m => m.PhoneCountry).MaximumLength(3);
        }
    }
}
