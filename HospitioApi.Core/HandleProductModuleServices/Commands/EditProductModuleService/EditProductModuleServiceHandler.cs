using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleProductModuleService.Commands.EditProductModuleService;

public record EditProductModuleServiceRequest(EditProductModuleServiceIn In, int Id)
    : IRequest<AppHandlerResponse>;

public class EditProductModuleServiceHandler : IRequestHandler<EditProductModuleServiceRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public EditProductModuleServiceHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(EditProductModuleServiceRequest request, CancellationToken cancellationToken)
    {
        var ProductModuleService = await _db.ProductModuleServices.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (ProductModuleService == null)
        {
            return _response.Error("Product module service not found.", AppStatusCodeError.Conflict409);
        }

        if (await _db.ProductModuleServices.AnyAsync(m => m.ProductModuleId == request.In.ProductModuleId && m.ProductId == request.In.ProductId && m.ModuleServiceId == request.In.ModuleServiceId, cancellationToken))
        {
            return _response.Error("The product module service is already exists in the system.", AppStatusCodeError.Conflict409);
        }

        ProductModuleService.ModuleServiceId = request.In.ModuleServiceId;
        ProductModuleService.ProductId = request.In.ProductId;
        ProductModuleService.ProductModuleId = request.In.ProductModuleId;

        _db.ProductModuleServices.Update(ProductModuleService);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new EditProductModuleServiceOut("Edit product module service successful."));
    }
}
