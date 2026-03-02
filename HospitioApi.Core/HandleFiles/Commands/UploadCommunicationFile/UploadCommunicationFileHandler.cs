using MediatR;
using HospitioApi.Core.HandleFiles.Commands.CreateFile;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleFiles.Commands.UploadCommunicationFile
{
    public record UploadCommunicationFileRequest(UploadCommunicationFileIn In) :  IRequest<AppHandlerResponse>;
    public class UploadCommunicationFileHandler : IRequestHandler<UploadCommunicationFileRequest, AppHandlerResponse>
    {
        private readonly IHandlerResponseFactory _response;
        private readonly IUserFilesService _userFilesService;
        private readonly ApplicationDbContext _db;

        public UploadCommunicationFileHandler(IHandlerResponseFactory response, IUserFilesService userFilesService, ApplicationDbContext db)
        {
            _response = response;
            _userFilesService = userFilesService;
            _db = db;
        }

        public async Task<AppHandlerResponse> Handle(UploadCommunicationFileRequest request, CancellationToken cancellationToken)
        {
            string filePath = "";
            //if (request.In.ContainerName is null)
            filePath = request.In.DocumentType;
            //else
            //    filePath = request.In.ContainerName + "\\" + request.In.DocumentType;

            var webFile = await _userFilesService.UploadWebFileOnGivenPathAsync(request.In.File, filePath, cancellationToken, false);

            if (webFile is null || webFile.MemoryStream is null || webFile.MemoryStream.Length <= 0)
            {
                return _response.Error("Unable to uploaded file.", AppStatusCodeError.InternalServerError500);
            }

            webFile.MemoryStream.Position = 0;

            return _response.Success(new UploadCommunicationFileOutV1("Communication File uploaded successfuly.", webFile.Name, webFile.Location, webFile.ContentType, webFile.TempSasUri, webFile.ExpireAt));

        }
    }
}
