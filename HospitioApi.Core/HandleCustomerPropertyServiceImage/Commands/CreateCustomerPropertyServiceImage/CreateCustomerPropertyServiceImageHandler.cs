using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerPropertyServiceImage.Commands.CreateCustomerPropertyServiceImage;
public record CreateCustomerPropertyServiceImageRequest(CreateCustomerPropertyServiceImageIn In, IFormFile File)
    : IRequest<AppHandlerResponse>;
public class CreateCustomerPropertyServiceImageHandler : IRequestHandler<CreateCustomerPropertyServiceImageRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly IUserFilesService _userFilesService;
    private readonly ApplicationDbContext _db;

    public CreateCustomerPropertyServiceImageHandler(
        IHandlerResponseFactory response,
        IUserFilesService userFilesService,
        ApplicationDbContext db)
    {
        _response = response;
        _userFilesService = userFilesService;
        _db = db;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomerPropertyServiceImageRequest request, CancellationToken cancellationToken)
    {
        var checkExists = await _db.CustomerPropertyServices.Where(x => x.Id == request.In.CustomerPropertyServiceId).FirstOrDefaultAsync(cancellationToken);

        if (checkExists == null)
        {
            return _response.Error($"The customer property service not found.", AppStatusCodeError.UnprocessableEntity422);
        }

        return _response.Success(new CreateCustomerPropertyServiceImageOut("Customer property service image created successful."));
    }
}
