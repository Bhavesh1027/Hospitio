using FluentValidation;

namespace HospitioApi.Core.HandleQuestionAnswer.Queries.GetQuestionAnswerInfoById;

public class GetQuestionAnswerByIdValidator : AbstractValidator<GetQuestionAnswerByIdRequest>
{
    public GetQuestionAnswerByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetQuestionAnswerByIdInValidator());
    }
    public class GetQuestionAnswerByIdInValidator : AbstractValidator<GetQuestionAnswerByIdIn>
    {
        public GetQuestionAnswerByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
