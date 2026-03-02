using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerPropertyServiceImage.Commands.DeleteCustomerPropertyServiceImage;
public record DeleteCustomerPropertyServiceImageRequest(DeleteCustomerPropertyServiceImageIn In)
    : IRequest<AppHandlerResponse>;
public class DeleteCustomerPropertyServiceImageHandler : IRequestHandler<DeleteCustomerPropertyServiceImageRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IUserFilesService _userFilesService;
    public DeleteCustomerPropertyServiceImageHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response, IUserFilesService userFilesService
        )
    {
        _db = db;
        _response = response;
        _userFilesService = userFilesService;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerPropertyServiceImageRequest request, CancellationToken cancellationToken)
    {
        var customerPropertyServiceImage = await _db.CustomerPropertyServiceImages.Where(c => c.Id == request.In.Id).SingleOrDefaultAsync(cancellationToken);
        if (customerPropertyServiceImage == null)
        {
            return _response.Error($"Customer property service image is not found with id {request.In.Id}", AppStatusCodeError.Gone410);
        }

        if (customerPropertyServiceImage.ServiceImages != null)
        {
            _db.CustomerPropertyServiceImages.Remove(customerPropertyServiceImage);
        }

        await _db.SaveChangesAsync(cancellationToken);

        RemoveCustomerPropertyServiceImageOut removeCustomerPropertyServiceImageOut = new() { DeletedCustomerPropertyServiceImageId = request.In.Id };
        return _response.Success(new DeleteCustomerPropertyServiceImageOut("Delete customer property image successfully.", removeCustomerPropertyServiceImageOut));
    }
}
