using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitioApi.Data.Models;

public partial class QuestionAnswer : Auditable
{
    public QuestionAnswer()
    {
        QuestionAnswerAttachements = new HashSet<QuestionAnswerAttachement>();
    }

    public int? QuestionAnswerCategoryId { get; set; }

    [MaxLength(20)]
    public string? Name { get; set; }

    [MaxLength(100)]
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public bool IsPublish { get; set; }

    [ForeignKey("QuestionAnswerCategoryId")]
    public virtual QuestionAnswerCategory? QuestionAnswerCategory { get; set; }
    public virtual ICollection<QuestionAnswerAttachement> QuestionAnswerAttachements { get; set; }
}
