using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleDisplayorder.Commands.UpdateDisplayorder;
public record UpdateDisplayorderRequest(UpdateDisplayorderIn In) : IRequest<AppHandlerResponse>;
public class UpdateDisplayorderHandle : IRequestHandler<UpdateDisplayorderRequest, AppHandlerResponse>
{

    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdateDisplayorderHandle(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateDisplayorderRequest request, CancellationToken cancellationToken)
    {
        var ScreenId = await _db.ScreenDisplayOrderAndStatuses.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (ScreenId == null)
        {
            return _response.Error($"Screen with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }
        ScreenId.ScreenName = request.In.ScreenName;
        ScreenId.JsonData = request.In.JsonData;
        ScreenId.RefrenceId = request.In.RefrenceId;

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdateDisplayorderOut("Update Successful."));
    }
}
