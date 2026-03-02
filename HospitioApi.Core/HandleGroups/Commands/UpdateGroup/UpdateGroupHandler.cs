using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGroups.Commands.UpdateGroup;
public record UpdateGroupRequest(UpdateGroupIn In) : IRequest<AppHandlerResponse>;
public class UpdateGroupHandler : IRequestHandler<UpdateGroupRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdateGroupHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateGroupRequest request, CancellationToken cancellationToken)
    {
        if (request.In.UserType == (int)UserTypeEnum.Hospitio)
        {
            var checkExist = await _db.Groups.Where(e => e.Name == request.In.Name && e.DepartmentId == request.In.DepartmentId).FirstOrDefaultAsync(cancellationToken);
            if (checkExist != null)
            {
                return _response.Error($"The group {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
            }

            var group = await _db.Groups.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
            if (group == null)
            {
                return _response.Error($"Group with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
            }
            group.Name = request.In.Name;
            group.DepartmentId = request.In.DepartmentId;
            group.IsActive = request.In.IsActive;
            await _db.SaveChangesAsync(cancellationToken);
        }
        else if(request.In.UserType == (int)UserTypeEnum.Customer)
        {
            var checkExist = await _db.CustomerGroups.Where(e => e.Name == request.In.Name && e.DepartmentId == request.In.DepartmentId).FirstOrDefaultAsync(cancellationToken);
            if (checkExist != null)
            {
                return _response.Error($"The group {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
            }

            var group = await _db.CustomerGroups.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
            if (group == null)
            {
                return _response.Error($"Group with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
            }
            group.Name = request.In.Name;
            group.DepartmentId = request.In.DepartmentId;
            group.IsActive = request.In.IsActive;
            await _db.SaveChangesAsync(cancellationToken);
        }

        var upadteGroupOut = new UpdatedGroupOut()
        {
            GroupId = request.In.Id,
            GroupName = request.In.Name!,
            DepartmentId = request.In.DepartmentId,
            IsActive = request.In.IsActive
        };
        return _response.Success(new UpdateGroupOut("Update group successful", upadteGroupOut));
    }
}

