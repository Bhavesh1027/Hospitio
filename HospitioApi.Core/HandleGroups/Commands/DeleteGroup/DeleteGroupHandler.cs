using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGroups.Commands.DeleteGroup;

public record DeleteGroupRequest(DeleteGroupIn In) : IRequest<AppHandlerResponse>;

public class DeleteGroupHandler : IRequestHandler<DeleteGroupRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteGroupHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteGroupRequest request, CancellationToken cancellationToken)
    {
        if (request.In.UserType == (int)UserTypeEnum.Hospitio)
        {
            var group = await _db.Groups.Where(e => e.Id == request.In.GroupId).SingleOrDefaultAsync(cancellationToken);
            if (group == null)
            {
                return _response.Error($"Group with {request.In.GroupId} not found.", AppStatusCodeError.Gone410);
            }
            _db.Groups.Remove(group);
            await _db.SaveChangesAsync(cancellationToken);
        }
        else if(request.In.UserType == (int)UserTypeEnum.Customer)
        {
            var group = await _db.CustomerGroups.Where(e => e.Id == request.In.GroupId).SingleOrDefaultAsync(cancellationToken);
            if (group == null)
            {
                return _response.Error($"Group with {request.In.GroupId} not found.", AppStatusCodeError.Gone410);
            }
            _db.CustomerGroups.Remove(group);
            await _db.SaveChangesAsync(cancellationToken);
        }

        DeleteGroup deleteGroup = new() { DeleteGroupId = request.In.GroupId };
        return _response.Success(new DeleteGroupOut("Delete group successful.", deleteGroup));
    }

}
