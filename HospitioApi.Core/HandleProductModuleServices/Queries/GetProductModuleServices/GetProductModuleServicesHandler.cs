using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleProductModuleService.Queries.GetProductModuleServices;

public record GetProductModuleServicesRequest()
    : IRequest<AppHandlerResponse>;

public class GetProductModuleServicesHandler : IRequestHandler<GetProductModuleServicesRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public GetProductModuleServicesHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetProductModuleServicesRequest request, CancellationToken cancellationToken)
    {
        var productModuleServices = await _db.ProductModuleServices.Include(x => x.Product).Include(x => x.ProductModule).Include(x => x.ModuleService).Select(x => new GetProductModuleServicesResponseOut()
        {
            Id = x.Id,
            ProductId = x.ProductId,
            ProductName = x.Product.Name,
            ModuleServiceId = x.ModuleServiceId,
            ModuleServiceName = x.ModuleService.Name,
            ProductModuleId = x.ModuleServiceId,

        }).ToListAsync(cancellationToken);

        if (!productModuleServices.Any())
        {
            return _response.Error("Product module services not found.", AppStatusCodeError.Conflict409);
        }

        return _response.Success(new GetProductModuleServicesOut("Get product module services successful.", productModuleServices));
    }
}
