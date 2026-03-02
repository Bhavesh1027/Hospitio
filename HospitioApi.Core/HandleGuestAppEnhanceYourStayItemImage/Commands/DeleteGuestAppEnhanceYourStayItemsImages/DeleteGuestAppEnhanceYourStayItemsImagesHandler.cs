using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Commands.DeleteGuestAppEnhanceYourStayItemsImages;
public record DeleteEnhanceYourStayItemsImageRequest(DeleteGuestAppEnhanceYourStayItemsImagesIn In)
    : IRequest<AppHandlerResponse>;
public class DeleteGuestAppEnhanceYourStayItemsImagesHandler : IRequestHandler<DeleteEnhanceYourStayItemsImageRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IUserFilesService _userFilesService;
    public DeleteGuestAppEnhanceYourStayItemsImagesHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response, IUserFilesService userFilesService
        )
    {
        _db = db;
        _response = response;
        _userFilesService = userFilesService;
    }

    public async Task<AppHandlerResponse> Handle(DeleteEnhanceYourStayItemsImageRequest request, CancellationToken cancellationToken)
    {
        var exist = await _db.CustomerGuestAppEnhanceYourStayItemsImages.Where(c => c.CustomerGuestAppEnhanceYourStayItemId == request.In.CustomerGuestAppEnhanceYourStayItemId).ToListAsync(cancellationToken);
        if (exist.Count == 0 || exist == null)
        {
            return _response.Error($"Enhance your stay item image article with CustomerGuestAppEnhanceYourStayItemId {request.In.CustomerGuestAppEnhanceYourStayItemId} not found or user doesn't have the rights to delete it", AppStatusCodeError.Gone410);
        }
        /** clean up old question answer attachements */
        if (exist != null)
        {
            while (exist.Count > 0)
            {
                var oldAttachments = exist.Last();
                exist.Remove(oldAttachments);
                _db.CustomerGuestAppEnhanceYourStayItemsImages.Remove(oldAttachments);
                //try
                //{
                //    await _userFilesService.DeleteBLOBFileAsync(oldAttachments.ItemsImages!, cancellationToken);
                //}
                //catch (Exception ex)
                //{

                //}
            }
        }
        await _db.SaveChangesAsync(cancellationToken);

        RemoveEnhanceYourStayItemImageOut removeQuestionAnswerOut = new() { CustomerGuestAppEnhanceYourStayItemId = request.In.CustomerGuestAppEnhanceYourStayItemId };
        return _response.Success(new DeleteGuestAppEnhanceYourStayItemsImagesOut("Delete article successfully.", removeQuestionAnswerOut));
    }
}