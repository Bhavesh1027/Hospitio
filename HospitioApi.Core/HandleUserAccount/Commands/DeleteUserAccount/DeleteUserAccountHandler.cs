using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleUserAccount.Commands.DeleteUserAccount;

public record DeleteUserAccountRequest(DeleteUserAccountIn In, bool IsAdmin) : IRequest<AppHandlerResponse>;

public class DeleteUserAccountHandler : IRequestHandler<DeleteUserAccountRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteUserAccountHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteUserAccountRequest request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.Where(e => e.Id == request.In.UserId).SingleOrDefaultAsync(cancellationToken);
        if (user == null)
        {
            return _response.Error($"Group with {request.In.UserId} not found.", AppStatusCodeError.Gone410);
        }
        _db.Users.Remove(user);
        await _db.SaveChangesAsync(cancellationToken);

        DeleteUser deleteUser = new() { DeleteUserId = request.In.UserId };
        return _response.Success(new DeleteUserAccountOut("Delete group successful.", deleteUser));
    }

}