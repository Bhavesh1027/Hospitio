using FluentValidation;

namespace HospitioApi.Core.HandleCustomers.Queries.GetLanguageTranslation;

public class GetLanguageTranslationValidation : AbstractValidator<GetLanguageTranslationRequest>
{
    public GetLanguageTranslationValidation()
    {
        RuleFor(m => m.channelMessageId).NotNull().NotEmpty().GreaterThan(0);
        RuleFor(m => m.message).NotNull().NotEmpty();
    }
}
