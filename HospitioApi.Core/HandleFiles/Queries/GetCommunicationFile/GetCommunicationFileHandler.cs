using MediatR;
using HospitioApi.Core.HandleFiles.Queries.GetFile;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleFiles.Queries.GetCommunicationFile
{
    public record GetCommunicationFileRequest(string Location)
    : IRequest<AppHandlerResponse>;
    public class GetCommunicationFileHandler : IRequestHandler<GetCommunicationFileRequest, AppHandlerResponse>
    {
        private readonly IHandlerResponseFactory _response;
        private readonly IUserFilesService _userFilesService;
        public GetCommunicationFileHandler(IHandlerResponseFactory response, IUserFilesService userFilesService)
        {
            _response = response;
            _userFilesService = userFilesService;
        }

        public async Task<AppHandlerResponse> Handle(GetCommunicationFileRequest request, CancellationToken cancellationToken)
        {
            var Location = request.Location;
            Location = Location.Replace("////", "//");
            Location = Location.Replace("\\\\", "\\");
            var getFileobj = await _userFilesService.GetFileTempSasURIAsync(Location, cancellationToken);

            var fileName = Path.GetFileName(Location);
            var extension = Path.GetExtension(Location);
            var contentType = _userFilesService.GetContentTypeFromExtension(extension);
            return _response.Success(new GetCommunicationFileOutV1("File found successful.",
                 fileName, contentType, getFileobj.TempSasUri, getFileobj.ExpireAt));
        }
    }
}
