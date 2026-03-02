using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.DeleteCustomersPropertiesInfo;
public record DeleteCustomersPropertiesInfoRequest(DeleteCustomersPropertiesInfoIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomersPropertiesInfoHandler : IRequestHandler<DeleteCustomersPropertiesInfoRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteCustomersPropertiesInfoHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomersPropertiesInfoRequest request, CancellationToken cancellationToken)
    {
        var customersPropertiesInfo = await _db.CustomerPropertyInformations.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customersPropertiesInfo == null)
        {
            return _response.Error($"Customers property info with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }
        _db.CustomerPropertyInformations.Remove(customersPropertiesInfo);
        await _db.SaveChangesAsync(cancellationToken);
        return _response.Success(new DeleteCustomersPropertiesInfoOut("Delete customers property info successful.", new() { Id = customersPropertiesInfo.Id }));
    }
}
