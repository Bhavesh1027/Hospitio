using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleProductModule.Commands.EditProductModule;

public record EditProductModuleRequest(EditProductModuleIn In, int Id)
    : IRequest<AppHandlerResponse>;

public class EditProductModuleHandler : IRequestHandler<EditProductModuleRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public EditProductModuleHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(EditProductModuleRequest request, CancellationToken cancellationToken)
    {
        var productModule = await _db.ProductModules.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (productModule == null)
        {
            return _response.Error("Product module not found.", AppStatusCodeError.Conflict409);
        }

        if (await _db.ProductModules.AnyAsync(m => m.ProductId == request.In.ProductId && m.ModuleId == request.In.ModuleId, cancellationToken))
        {
            return _response.Error("The product module is already exists in the system.", AppStatusCodeError.Conflict409);
        }

        productModule.Price = request.In.Price;
        productModule.ProductId = request.In.ProductId;
        productModule.ModuleId = request.In.ModuleId;
        productModule.Currency = request.In.Currency;

        _db.ProductModules.Update(productModule);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new EditProductModuleOut("Edit product module successful."));
    }
}
