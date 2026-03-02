using FluentValidation;

namespace HospitioApi.Core.HandleQaCategories.Commands.DeleteQaCategory;

public class DeleteQaCategoryValidator : AbstractValidator<DeleteQaCategoryRequest>
{
    public DeleteQaCategoryValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteQaCategoryInValidator());
    }
    public class DeleteQaCategoryInValidator : AbstractValidator<DeleteQaCategoryIn>
    {
        public DeleteQaCategoryInValidator()
        {
            RuleFor(m => m.QaCategoryId).NotEmpty().GreaterThan(0);
        }
    }
}
