using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleDepartment.Commands.EditDepartment;

public record EditDepartmentRequest(EditDepartmentIn In, int Id)
    : IRequest<AppHandlerResponse>;

public class EditDepartmentHandler : IRequestHandler<EditDepartmentRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public EditDepartmentHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(EditDepartmentRequest request, CancellationToken cancellationToken)
    {
        if (request.In.UserType == (int)UserTypeEnum.Hospitio)
        {
            var department = await _db.Departments.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (department == null)
            {
                return _response.Error("Department not found.", AppStatusCodeError.Conflict409);
            }

            department.Name = request.In.Name;
            department.IsActive = request.In.IsActive;

            _db.Departments.Update(department);
            await _db.SaveChangesAsync(cancellationToken);

            return _response.Success(new EditDepartmentOut("Edit department successful."));
        }
        else if(request.In.UserType == (int)UserTypeEnum.Customer)
        {
            var customerdepartment = await _db.CustomerDepartments.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (customerdepartment == null)
            {
                return _response.Error("Department not found.", AppStatusCodeError.Conflict409);
            }

            customerdepartment.Name = request.In.Name;
            customerdepartment.IsActive = request.In.IsActive;

            _db.CustomerDepartments.Update(customerdepartment);
            await _db.SaveChangesAsync(cancellationToken);

            return _response.Success(new EditDepartmentOut("Edit department successful."));
        }

        return _response.Error("Department doesn't update" , AppStatusCodeError.Conflict409);
    }
}
