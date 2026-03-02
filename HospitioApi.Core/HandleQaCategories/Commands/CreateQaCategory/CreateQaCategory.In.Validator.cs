using FluentValidation;

namespace HospitioApi.Core.HandleQaCategories.Commands.CreateQaCategory;

public class CreateQaCategoryValidator : AbstractValidator<CreateQaCategoryRequest>
{
    public CreateQaCategoryValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateQaCategoryInValidator());
    }
    public class CreateQaCategoryInValidator : AbstractValidator<CreateQaCategoryIn>
    {
        public CreateQaCategoryInValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}
