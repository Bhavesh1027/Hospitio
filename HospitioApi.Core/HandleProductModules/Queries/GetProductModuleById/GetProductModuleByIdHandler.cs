using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleProductModule.Queries.GetProductModuleById;

public record GetProductModuleByIdRequest(int Id)
    : IRequest<AppHandlerResponse>;

public class GetProductModuleByIdHandler : IRequestHandler<GetProductModuleByIdRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public GetProductModuleByIdHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetProductModuleByIdRequest request, CancellationToken cancellationToken)
    {
        var productModule = await _db.ProductModules.Include(x => x.Product).Include(x => x.Module).FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (productModule == null)
        {
            return _response.Error("Product module not found.", AppStatusCodeError.Conflict409);
        }

        var getProductModuleByIdResponseOut = new GetProductModuleByIdResponseOut()
        {
            Id = productModule.Id,
            ModuleId = productModule.ModuleId,
            ModuleName = productModule?.Module?.Name,
            ProductId = productModule.ProductId,
            ProductName = productModule?.Product?.Name,
            Currency = productModule?.Currency,
            Price = productModule.Price
        };

        return _response.Success(new GetProductModuleByIdOut("Get product module successful.", getProductModuleByIdResponseOut));
    }
}
