using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Commands.DeleteGuestAppEnhanceYourStayItemsImages;

public class DeleteGuestAppEnhanceYourStayItemsImagesOut : BaseResponseOut
{
    public DeleteGuestAppEnhanceYourStayItemsImagesOut(string message, RemoveEnhanceYourStayItemImageOut removeQuestionAnswerOut) : base(message)
    {
        RemoveQuestionAnswerOut = removeQuestionAnswerOut;
    }
    public RemoveEnhanceYourStayItemImageOut RemoveQuestionAnswerOut { get; set; }
}
public class RemoveEnhanceYourStayItemImageOut
{
    public int CustomerGuestAppEnhanceYourStayItemId { get; set; }
}