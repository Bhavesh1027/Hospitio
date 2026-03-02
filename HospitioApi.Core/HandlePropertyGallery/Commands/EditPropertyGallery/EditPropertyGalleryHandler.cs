using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandlePropertyGallery.Commands.EditPropertyGallery;
public record EditPropertyGalleryRequest(EditPropertyGalleryIn In)
: IRequest<AppHandlerResponse>;

public class EditPropertyGalleryHandler : IRequestHandler<EditPropertyGalleryRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IUserFilesService _userFilesService;
    public EditPropertyGalleryHandler(
        ApplicationDbContext db, IUserFilesService userFilesService,
        IHandlerResponseFactory response)
    {
        _db = db;
        _userFilesService = userFilesService;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(EditPropertyGalleryRequest request, CancellationToken cancellationToken)
    {
        if (request.In == null)
        {
            return _response.Error($"Request cannot be null.", AppStatusCodeError.Forbidden403);
        }

        var In = request.In;
        var checkExist = await _db.CustomerPropertyGalleries.Where(e => e.Id == In.Id && e.CustomerPropertyInformationId == In.CustomerPropertyInformationId).FirstOrDefaultAsync(cancellationToken);

        if (checkExist == null)
        {
            return _response.Error($"Given property Id not exists.", AppStatusCodeError.UnprocessableEntity422);
        }

        checkExist.PropertyImage = In.PropertyImages;
        checkExist.IsActive = true;

        _db.CustomerPropertyGalleries.Update(checkExist);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new EditPropertyGalleryOut("Gallery edited successfully."));
    }
}