using HospitioApi.Shared;


namespace HospitioApi.Core.HandleQuestionAnswer.Commands.EditQuestionAnswer;

public class EditQuestionAnswerOut : BaseResponseOut
{
    public EditQuestionAnswerOut(string message, EditedQuestionAnswerOut editedQuestionAnswerOut) : base(message)
    {
        EditedQuestionAnswerOut = editedQuestionAnswerOut;
    }
    public EditedQuestionAnswerOut EditedQuestionAnswerOut { get; set; }

}
public class EditedQuestionAnswerOut
{
    public int Id { get; set; }

}