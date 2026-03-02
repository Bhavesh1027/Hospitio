using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleProductModule.Commands.CreateProductModule;

public record CreateProductModuleRequest(CreateProductModuleIn In)
    : IRequest<AppHandlerResponse>;

public class CreateProductModuleHandler : IRequestHandler<CreateProductModuleRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateProductModuleHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateProductModuleRequest request, CancellationToken cancellationToken)
    {

        if (await _db.ProductModules.AnyAsync(m => m.ProductId == request.In.ProductId && m.ModuleId == request.In.ModuleId, cancellationToken))
        {
            return _response.Error("The product module is already exists in the system.", AppStatusCodeError.Conflict409);
        }

        var ProductModule = new Data.Models.ProductModule()
        {
            ModuleId = request.In.ModuleId,
            ProductId = request.In.ProductId,
            Price = request.In.Price,
            Currency = request.In.Currency,
            IsActive = true,
        };

        await _db.ProductModules.AddAsync(ProductModule, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new CreateProductModuleOut("Create product module successful."));
    }
}
