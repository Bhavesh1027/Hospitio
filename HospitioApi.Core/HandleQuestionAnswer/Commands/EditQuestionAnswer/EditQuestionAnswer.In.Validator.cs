using FluentValidation;


namespace HospitioApi.Core.HandleQuestionAnswer.Commands.EditQuestionAnswer;

public class EditQuestionAnswerValidator : AbstractValidator<EditQuestionAnswerRequest>
{
    public EditQuestionAnswerValidator()
    {
        RuleFor(m => m.In).SetValidator(new EditQuestionAnswerInValidator());
    }
    public class EditQuestionAnswerInValidator : AbstractValidator<EditQuestionAnswerIn>
    {
        public EditQuestionAnswerInValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}