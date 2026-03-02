using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class QuestionAnswerCategory : Auditable
{
    public QuestionAnswerCategory()
    {
        QuestionAnswers = new HashSet<QuestionAnswer>();
    }

    [MaxLength(50)]
    public string? Name { get; set; }


    public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
}
