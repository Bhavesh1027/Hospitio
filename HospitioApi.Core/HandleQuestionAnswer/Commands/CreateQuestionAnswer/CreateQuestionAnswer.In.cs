

namespace HospitioApi.Core.HandleQuestionAnswer.Commands.CreateQuestionAnswer
{
    public class CreateQuestionAnswerIn
    {
        public int Id { get; set; }
        public int QuestionAnswerCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        public bool IsActive { get; set; }
        public bool IsPublish { get; set; }

        public List<QuestionAnswerAttachementIn> QuestionAnswerAttachements { get; set; } = new();
    }

    public class QuestionAnswerAttachementIn
    {
        public int? QuestionAnswerId { get; set; }
        public string? Attachment { get; set; }
        public string? AttachmentType { get; set; }
    }
}

