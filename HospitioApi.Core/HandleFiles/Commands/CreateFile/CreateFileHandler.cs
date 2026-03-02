
using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleFiles.Commands.CreateFile;
public record CreateFileRequest(CreateFileIn In)
    : IRequest<AppHandlerResponse>;

public class CreateFileHandler : IRequestHandler<CreateFileRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly IUserFilesService _userFilesService;
    private readonly ApplicationDbContext _db;

    public CreateFileHandler(
        IHandlerResponseFactory response,
        IUserFilesService userFilesService,
        ApplicationDbContext db)
    {
        _response = response;
        _userFilesService = userFilesService;
        _db = db;
    }

    public async Task<AppHandlerResponse> Handle(CreateFileRequest request, CancellationToken cancellationToken)
    {
        string filePath = "";
        if (request.In.ContainerName is null)
            filePath = request.In.DocumentType;
        else
            filePath = request.In.ContainerName + "\\" + request.In.DocumentType;

        var webFile = await _userFilesService.UploadWebFileOnGivenPathAsync(request.In.File, filePath, cancellationToken, false);

        if (webFile is null || webFile.MemoryStream is null || webFile.MemoryStream.Length <= 0)
        {
            return _response.Error("Unable to uploaded file.", AppStatusCodeError.InternalServerError500);
        }

        webFile.MemoryStream.Position = 0;
        return _response.Success(new CreateFileOutV1("File uploaded successfuly.", webFile.Name, webFile.Location, webFile.ContentType, webFile.TempSasUri, webFile.ExpireAt));
    }
}
