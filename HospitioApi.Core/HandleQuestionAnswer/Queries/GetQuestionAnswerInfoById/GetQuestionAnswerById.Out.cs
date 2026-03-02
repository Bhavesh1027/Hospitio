using HospitioApi.Shared;

namespace HospitioApi.Core.HandleQuestionAnswer.Queries.GetQuestionAnswerInfoById;

public class GetQuestionAnswerByIdOut : BaseResponseOut
{
    public GetQuestionAnswerByIdOut(string message, QuestionAnswerByIdOut questionAnswerByIdOut) : base(message)
    {
        QuestionAnswerByIdOut = questionAnswerByIdOut;
    }
    public QuestionAnswerByIdOut QuestionAnswerByIdOut { get; set; }
}
public class QuestionAnswerByIdOut
{
    public int Id { get; set; }
    public int? QuestionAnswerCategoryId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsPublish { get; set; }
    public List<QuestionAnswerAttachements> QuestionAnswerAttachements { get; set; } = new();
}
public class QuestionAnswerAttachements
{
    public int QuestionAnswerAttachementId { get; set; }
    public string? Attachment { get; set; }
    public string? AttachmentType { get; set; }
}