using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerUsers.Commands.UpdateCustomerUserStatus;
public record UpdateCustomerUserStatusRequest(UpdateCustomerUserStatusIn In)
    : IRequest<AppHandlerResponse>;
public class UpdateCustomerUserStatusHandler : IRequestHandler<UpdateCustomerUserStatusRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public UpdateCustomerUserStatusHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateCustomerUserStatusRequest request, CancellationToken cancellationToken)
    {
        var CustomerUserRequest = await _db.CustomerUsers.Where(r => r.Id == request.In.CustomerUserId).SingleOrDefaultAsync(cancellationToken);
        if (CustomerUserRequest == null)
        {
            return _response.Error("Customer User not found.", AppStatusCodeError.Gone410);
        }
        CustomerUserRequest.IsActive = request.In.IsActive;
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdateCustomerUserStatusOut("Update CustomerUser status successfully."));
    }
}
