using FluentValidation;

namespace HospitioApi.Core.HandleQuestionAnswer.Commands.CreateQuestionAnswer;

public class CreateQuestionAnswerValidator : AbstractValidator<CreateQuestionAnswerRequest>
{
    public CreateQuestionAnswerValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateQuestionAnswerInValidator());
    }
    public class CreateQuestionAnswerInValidator : AbstractValidator<CreateQuestionAnswerIn>
    {
        public CreateQuestionAnswerInValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}