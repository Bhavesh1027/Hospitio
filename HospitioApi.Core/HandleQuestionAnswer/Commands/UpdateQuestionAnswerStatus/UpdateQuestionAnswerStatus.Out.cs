

using HospitioApi.Shared;

namespace HospitioApi.Core.HandleQuestionAnswer.Commands.UpdateQuestionAnswerStatus;

public class UpdateQuestionAnswerStatusOut : BaseResponseOut
{
    public UpdateQuestionAnswerStatusOut(string message) : base(message)
    {
        // UpdatedQuestionAnswerStatusOut = updatedQuestionAnswerStatusOut;
    }
    //public UpdatedQuestionAnswerStatusOut UpdatedQuestionAnswerStatusOut { get; set; } = new();
}

//public class UpdatedQuestionAnswerStatusOut
//{
//    public int Id { get; set; }
//    public string Name { get; set; } = string.Empty;
//}