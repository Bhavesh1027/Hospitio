
using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleFiles.Queries.GetFile;


public record GetFileRequest(string Location)
    : IRequest<AppHandlerResponse>;

public class GetFileHandler : IRequestHandler<GetFileRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly IUserFilesService _userFilesService;

    public GetFileHandler(
        IHandlerResponseFactory response,
        IUserFilesService userFilesService
        )
    {
        _response = response;
        _userFilesService = userFilesService;
    }

    public async Task<AppHandlerResponse> Handle(GetFileRequest request, CancellationToken cancellationToken)
    {
        var Location = request.Location;
        Location = Location.Replace("////", "//");
        Location = Location.Replace("\\\\", "\\");
        var getFileobj = await _userFilesService.GetFileTempSasURIAsync(Location, cancellationToken);

        var fileName = Path.GetFileName(Location);
        var extension = Path.GetExtension(Location);
        var contentType = _userFilesService.GetContentTypeFromExtension(extension);
        return _response.Success(new GetFileOutV1("File found successful.",
             fileName, contentType, getFileobj.TempSasUri, getFileobj.ExpireAt));

    }
}
