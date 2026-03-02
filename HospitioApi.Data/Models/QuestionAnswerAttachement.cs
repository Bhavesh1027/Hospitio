using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class QuestionAnswerAttachement : Auditable
{
    public int? QuestionAnswerId { get; set; }
    [MaxLength(500)]
    public string? Attachment { get; set; }
    [MaxLength(50)]
    public string? AttachmentType { get; set; }
    public virtual QuestionAnswer? QuestionAnswer { get; set; }
}
