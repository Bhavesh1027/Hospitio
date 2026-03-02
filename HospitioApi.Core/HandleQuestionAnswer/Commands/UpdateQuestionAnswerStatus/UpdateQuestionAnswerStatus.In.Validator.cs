

using FluentValidation;

namespace HospitioApi.Core.HandleQuestionAnswer.Commands.UpdateQuestionAnswerStatus;


public class UpdateQuestionAnswerStatusValidator : AbstractValidator<UpdateQuestionAnswerStatusRequest>
{
    public UpdateQuestionAnswerStatusValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateQuestionAnswerStatusInValidator());
    }
    public class UpdateQuestionAnswerStatusInValidator : AbstractValidator<UpdateQuestionAnswerStatusIn>
    {
        public UpdateQuestionAnswerStatusInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}