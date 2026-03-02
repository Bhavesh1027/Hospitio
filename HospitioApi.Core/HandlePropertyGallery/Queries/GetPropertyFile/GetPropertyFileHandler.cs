using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;


namespace HospitioApi.Core.HandlePropertyGallery.Queries.GetPropertyFile;

public record GetPropertyFileRequest(GetPropertyFileIn In)
    : IRequest<AppHandlerResponse>;

public class GetPropertyFileHandler : IRequestHandler<GetPropertyFileRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly IUserFilesService _userFilesService;
    private readonly ApplicationDbContext _db;

    public GetPropertyFileHandler(
        IHandlerResponseFactory response,
        IUserFilesService userFilesService,
        ApplicationDbContext db)
    {
        _response = response;
        _userFilesService = userFilesService;
        _db = db;
    }

    public async Task<AppHandlerResponse> Handle(GetPropertyFileRequest request, CancellationToken cancellationToken)
    {


        if (request.In.CustomerPropertyInformationId <= 0)
        {
            return _response.Error("Invalid request.", AppStatusCodeError.Conflict409);
        }

        var checkExist = await _db.CustomerPropertyGalleries.Where(e => e.Id == request.In.Id && e.CustomerPropertyInformationId == request.In.CustomerPropertyInformationId).SingleOrDefaultAsync(cancellationToken);
        if (checkExist == null)
        {
            return _response.Error($"Given property Id not exists.", AppStatusCodeError.UnprocessableEntity422);
        }

        var location = checkExist.PropertyImage;
        if (location is not null)
        {
            var memoryStreamFile = await _userFilesService.GetFileAsync(location, cancellationToken);
            if (memoryStreamFile is null)
            {
                return _response.Error($"No file location found for Profile.", AppStatusCodeError.Gone410);
            }
            var fileName = Path.GetFileName(location);
            var extension = Path.GetExtension(location);
            var contentType = _userFilesService.GetContentTypeFromExtension(extension);
            return _response.Success(new GetPropertyFileOut("File found successful.",
                memoryStreamFile, fileName, checkExist.Id, contentType));
        }
        return _response.Error($"No file location found.", AppStatusCodeError.Gone410);
    }
}
