using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleDepartment.Commands.CreateCustomerDepartment;

public record CreateCustomerDepartmentRequest(CreateCustomerDepartmentIn In)
    : IRequest<AppHandlerResponse>;
public class CreateCustomerDepartmentHandler :IRequestHandler<CreateCustomerDepartmentRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateCustomerDepartmentHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomerDepartmentRequest request, CancellationToken cancellationToken)
    {

        if (await _db.CustomerDepartments.AnyAsync(m => m.Name == request.In.Name && m.CustomerId == request.In.CustomerId, cancellationToken))
        {
            return _response.Error("The department name already exists in the system.", AppStatusCodeError.Conflict409);
        }

        var customerUserDetalis = await _db.CustomerUsers.Where(s => s.CustomerId == request.In.CustomerId).FirstOrDefaultAsync();

        var CustomerDepartment = new Data.Models.CustomerDepartment()
        {
            CreatedBy = customerUserDetalis?.Id,
            Name = request.In.Name,
            IsActive = request.In.IsActive,
            CustomerId = request.In.CustomerId
        };

        await _db.CustomerDepartments.AddAsync(CustomerDepartment, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new CreateCustomerDepartmentOut("Create department successful."));
    }
}
