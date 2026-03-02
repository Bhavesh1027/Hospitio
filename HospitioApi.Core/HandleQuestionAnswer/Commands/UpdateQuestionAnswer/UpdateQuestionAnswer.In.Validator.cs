

using FluentValidation;

namespace HospitioApi.Core.HandleQuestionAnswer.Commands.UpdateQuestionAnswer;

public class UpdateQuestionAnswerValidator : AbstractValidator<UpdateQuestionAnswerRequest>
{
    public UpdateQuestionAnswerValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateQuestionAnswerInValidator());
    }
    public class UpdateQuestionAnswerInValidator : AbstractValidator<UpdateQuestionAnswerIn>
    {
        public UpdateQuestionAnswerInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}