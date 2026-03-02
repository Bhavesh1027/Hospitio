using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleDepartment.Commands.CreateDepartment;

public record CreateDepartmentRequest(CreateDepartmentIn In)
    : IRequest<AppHandlerResponse>;

public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateDepartmentHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateDepartmentRequest request, CancellationToken cancellationToken)
    {

        if (await _db.Departments.AnyAsync(m => m.Name == request.In.Name, cancellationToken))
        {
            return _response.Error("The department name already exists in the system.", AppStatusCodeError.Conflict409);
        }

        var department = new Data.Models.Department()
        {
            CreatedBy = 1,
            Name = request.In.Name,
            IsActive = request.In.IsActive,
        };

        await _db.Departments.AddAsync(department, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new CreateDepartmentOut("Create department successful."));
    }
}
