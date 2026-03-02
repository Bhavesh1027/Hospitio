using FluentValidation;

namespace HospitioApi.Core.HandleQaCategories.Commands.UpdateQaCategory;

public class UpdateQaCategoryValidator : AbstractValidator<UpdateQaCategoryRequest>
{
    public UpdateQaCategoryValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateQaCategoryInValidator());
    }
}
public class UpdateQaCategoryInValidator : AbstractValidator<UpdateQaCategoryIn>
{
    public UpdateQaCategoryInValidator()
    {
        RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        RuleFor(m => m.Name).NotEmpty();
    }
}