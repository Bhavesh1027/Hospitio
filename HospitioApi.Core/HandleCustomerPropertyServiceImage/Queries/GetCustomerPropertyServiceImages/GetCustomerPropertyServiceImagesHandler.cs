using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerPropertyServiceImage.Queries.GetCustomerPropertyServiceImages;
public record GetCustomerPropertyServiceImagesRequest(GetCustomerPropertyServiceImagesIn In)
    : IRequest<AppHandlerResponse>;
public class GetCustomerPropertyServiceImagesHandler : IRequestHandler<GetCustomerPropertyServiceImagesRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly IUserFilesService _userFilesService;
    private readonly ApplicationDbContext _db;

    public GetCustomerPropertyServiceImagesHandler(
        IHandlerResponseFactory response,
        IUserFilesService userFilesService,
        ApplicationDbContext db)
    {
        _response = response;
        _userFilesService = userFilesService;
        _db = db;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerPropertyServiceImagesRequest request, CancellationToken cancellationToken)
    {
        var customerPropertyServiceImage = await _db.CustomerPropertyServiceImages.Where(x => x.CustomerPropertyServiceId == request.In.CustomerPropertyServiceId).ToListAsync(cancellationToken);

        if (!customerPropertyServiceImage.Any())
        {
            return _response.Error($"No customer property service images found.", AppStatusCodeError.Gone410);
        }

        //var customerPropertyServiceImages = new List<CustomerPropertyServiceImagesOut>();

        //foreach (var image in customerPropertyServiceImage)
        //{
        //    if (image.ServiceImages is not null)
        //    {
        //        var memoryStreamFile = await _userFilesService.GetFileAsync(image.ServiceImages, cancellationToken);
        //        if (memoryStreamFile is null)
        //        {
        //            return _response.Error($"No file location found for Profile.", AppStatusCodeError.Gone410);
        //        }
        //        var fileName = Path.GetFileName(image.ServiceImages);
        //        var extension = Path.GetExtension(image.ServiceImages);
        //        var contentType = _userFilesService.GetContentTypeFromExtension(extension);

        //        var imageObj = new CustomerPropertyServiceImagesOut
        //        {
        //            MemoryStream = memoryStreamFile,
        //            FileName = fileName,
        //            ContentType = contentType
        //        };

        //        customerPropertyServiceImages.Add(imageObj);
        //    }
        //}

        //if (customerPropertyServiceImages.Any())
        //{
        //    return _response.Success(new GetCustomerPropertyServiceImagesOut("Customer property service images found successful.", customerPropertyServiceImages));
        //}

        return _response.Error($"No customer property service images found.", AppStatusCodeError.Gone410);
    }
}
