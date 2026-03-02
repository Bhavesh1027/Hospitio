using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleUserAccount.Commands.UpdateUserStatus;
public record UpdateUserStatusRequest(UpdateUserStatusIn In)
    : IRequest<AppHandlerResponse>;

public class UpdateUserStatusHandler : IRequestHandler<UpdateUserStatusRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public UpdateUserStatusHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateUserStatusRequest request, CancellationToken cancellationToken)
    {
        var UserRequest = await _db.Users.Where(r => r.Id == request.In.UserId).SingleOrDefaultAsync(cancellationToken);
        if (UserRequest == null)
        {
            return _response.Error("User request not found.", AppStatusCodeError.Gone410);
        }
        UserRequest.IsActive = request.In.IsActive;
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdateUserStatusOut("Update request status successfully."));
    }
}