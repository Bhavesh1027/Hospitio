using FluentValidation;

namespace HospitioApi.Core.HandleQuestionAnswer.Queries.GetQuestionAnswers;

public class GetQuestionAnswers : AbstractValidator<GetQuestionAnswersRequest>
{
    public GetQuestionAnswers()
    {
        RuleFor(e => e.In).SetValidator(new GetQuestionAnswerInValidator());
    }
    public class GetQuestionAnswerInValidator : AbstractValidator<GetQuestionAnswersIn>
    {
        public GetQuestionAnswerInValidator()
        {
            //RuleFor(m => m.CategoryId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
        }
    }
}
