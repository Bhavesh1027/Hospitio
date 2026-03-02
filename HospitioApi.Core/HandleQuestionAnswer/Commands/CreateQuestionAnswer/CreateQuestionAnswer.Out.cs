using HospitioApi.Shared;


namespace HospitioApi.Core.HandleQuestionAnswer.Commands.CreateQuestionAnswer;

public class CreateQuestionAnswerOut : BaseResponseOut
{
    public CreateQuestionAnswerOut(string message, CreatedQuestionAnswerOut createdQuestionAnswerOut) : base(message)
    {
        CreatedQuestionAnswerOut = createdQuestionAnswerOut;
    }
    public CreatedQuestionAnswerOut CreatedQuestionAnswerOut { get; set; }

}
public class CreatedQuestionAnswerOut
{
    public int Id { get; set; }

}