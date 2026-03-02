using HospitioApi.Shared;


namespace HospitioApi.Core.HandleQuestionAnswer.Queries.GetQuestionAnswers;

public class GetQuestionAnswersOut : BaseResponseOut
{
    public GetQuestionAnswersOut(string message, List<QuestionAnswersOut> questionAnswersInfoOut) : base(message)
    {
        CustomersQuestionAnswersOut = questionAnswersInfoOut;
    }
    public List<QuestionAnswersOut> CustomersQuestionAnswersOut { get; set; } = new();
}

public class QuestionAnswersOut
{
    public int Id { get; set; }
    public int? QuestionAnswerCategoryId { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? Icon { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
    public bool? IsPublish { get; set; }
    public int? FilteredCount { get; set; }
    public int QuestionAnswerAttachementId { get; set; }
    public string? Attachment { get; set; }
    public string? AttachmentType { get; set; }
    public string? Status { get; set; }
}