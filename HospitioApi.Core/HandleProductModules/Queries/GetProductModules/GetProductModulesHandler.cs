using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleProductModule.Queries.GetProductModules;

public record GetProductModulesRequest()
    : IRequest<AppHandlerResponse>;

public class GetProductModulesHandler : IRequestHandler<GetProductModulesRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public GetProductModulesHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetProductModulesRequest request, CancellationToken cancellationToken)
    {
        var productModules = await _db.ProductModules.Select(x => new GetProductModulesResponseOut()
        {
            Id = x.Id,
            ModuleId = x.ModuleId,
            ModuleName = x.Module.Name,
            ProductId = x.ProductId,
            ProductName = x.Product.Name,
            Currency = x.Currency,
            Price = x.Price

        }).ToListAsync(cancellationToken);

        if (!productModules.Any())
        {
            return _response.Error("Product modules not found.", AppStatusCodeError.Conflict409);
        }

        return _response.Success(new GetProductModulesOut("Get product modules successful.", productModules));
    }
}
