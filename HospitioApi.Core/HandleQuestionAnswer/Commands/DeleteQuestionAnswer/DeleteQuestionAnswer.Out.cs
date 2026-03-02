using HospitioApi.Shared;

namespace HospitioApi.Core.HandleQuestionAnswer.Commands.DeleteQuestionAnswer;

public class DeleteQuestionAnswerOut : BaseResponseOut
{
    public DeleteQuestionAnswerOut(string message, RemoveQuestionAnswerOut removeQuestionAnswerOut) : base(message)
    {
        RemoveQuestionAnswerOut = removeQuestionAnswerOut;
    }
    public RemoveQuestionAnswerOut RemoveQuestionAnswerOut { get; set; }
}

public class RemoveQuestionAnswerOut
{
    public int DeletedQuestionAnswerId { get; set; }
}
