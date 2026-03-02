using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGroups.Commands.CreateGroup;
public record CreateGroupRequest(CreateGroupIn In) : IRequest<AppHandlerResponse>;
public class CreateGroupHandler : IRequestHandler<CreateGroupRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public CreateGroupHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
    {
        if (request.In.UserType == (int)UserTypeEnum.Hospitio)
        {

            var checkExist = await _db.Groups.Where(e => e.Name == request.In.Name && e.DepartmentId == request.In.DepartmentId).FirstOrDefaultAsync(cancellationToken);
            if (checkExist != null)
            {
                return _response.Error($"The group {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
            }
            var group = new Group
            {
                Name = request.In.Name,
                DepartmentId = request.In.DepartmentId,
                IsActive = request.In.IsActive
            };
            await _db.Groups.AddAsync(group, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            var createdGroupOut = new CreatedGroupOut
            {
                Id = request.In.Id,
                Name = request.In.Name,
                DepartmentId = request.In.DepartmentId,
                IsActive = request.In.IsActive
            };
            return _response.Success(new CreateGroupOut("Create group successful.", createdGroupOut));
        }
        else if (request.In.UserType == (int)UserTypeEnum.Customer)
        {
            var checkExist = await _db.CustomerGroups.Where(e => e.Name == request.In.Name && e.DepartmentId == request.In.DepartmentId).FirstOrDefaultAsync(cancellationToken);
            if (checkExist != null)
            {
                return _response.Error($"The group {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
            }
            var group = new CustomerGroup
            {
                Name = request.In.Name,
                DepartmentId = request.In.DepartmentId,
                IsActive = request.In.IsActive,
                CustomerId = request.In.UserId
            };
            await _db.CustomerGroups.AddAsync(group, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            var createdGroupOut = new CreatedGroupOut
            {
                Id = request.In.Id,
                Name = request.In.Name,
                DepartmentId = request.In.DepartmentId,
                IsActive = request.In.IsActive
            };
            return _response.Success(new CreateGroupOut("Create group successful.", createdGroupOut));
        }
        return _response.Error($"The group doesn't create.", AppStatusCodeError.UnprocessableEntity422);
    }
}
