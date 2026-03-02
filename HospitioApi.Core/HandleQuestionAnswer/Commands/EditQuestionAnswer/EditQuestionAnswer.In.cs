namespace HospitioApi.Core.HandleQuestionAnswer.Commands.EditQuestionAnswer;

public class EditQuestionAnswerIn
{
    public int Id { get; set; }
    public int QuestionAnswerCategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;

    public bool IsActive { get; set; }
    public bool IsPublish { get; set; }

    public List<EQuestionAnswerAttachementIn> QuestionAnswerAttachements { get; set; } = new();
}

public class EQuestionAnswerAttachementIn
{
    public int? QuestionAnswerId { get; set; }
    public string? Attachment { get; set; }
    public string? AttachmentType { get; set; }
}