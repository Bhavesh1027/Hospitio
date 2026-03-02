using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleProduct.Queries.GetProductById;

public record GetProductByIdRequest(int Id, string UserId)
    : IRequest<AppHandlerResponse>;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public GetProductByIdHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var product = await _db.Products.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);
        var modules = await _db.Modules.Include(x => x.ModuleServices).Where(x => x.IsActive == true).ToListAsync(cancellationToken);

        var productModules = await _db.ProductModules
            .Include(x => x.ProductModuleServices)
            .ThenInclude(x => x.ModuleService)
            .ThenInclude(x => x.Module)
            .Include(x => x.Module).Where(x => x.ProductId == request.Id).ToListAsync(cancellationToken);

        var getProductByIdResponseOut = new GetProductByIdResponseOut();
        var userData = await _db.Users.FirstOrDefaultAsync(m => m.Id == Int32.Parse(request.UserId), cancellationToken);
        getProductByIdResponseOut.CreatedBy = userData.FirstName + " " + userData.LastName;
        if (product != null)
        {
            getProductByIdResponseOut.ProductId = product.Id;
            getProductByIdResponseOut.ProductName = product.Name;
            getProductByIdResponseOut.CreatedAt = product.CreatedAt;
            getProductByIdResponseOut.UpdatedAt = product.UpdateAt;
            getProductByIdResponseOut.IsActive = product.IsActive;
            if (request.Id > 0)
            {
                userData = await _db.Users.FirstOrDefaultAsync(m => m.Id == product.CreatedBy, cancellationToken);
                getProductByIdResponseOut.CreatedBy = userData.FirstName + " " + userData.LastName;
            }
        }

        foreach (var module in modules)
        {
            var existproductmodule = productModules.Find(x => x.ModuleId == module.Id);
            var productModule = new ProductModule();

            if (existproductmodule != null)
            {
                productModule = new ProductModule()
                {
                    Id = existproductmodule.Id,
                    ModuleId = existproductmodule.ModuleId,
                    ModuleType = existproductmodule.Module?.ModuleType,
                    Module2TypeValue = existproductmodule.Module2TypeValue,
                    ProductId = existproductmodule.ProductId,
                    IsActive = existproductmodule.IsActive.HasValue ? existproductmodule.IsActive.Value : false,
                    ModuleName = module.Name,
                    Currency = existproductmodule.Currency,
                    Price = existproductmodule.Price,
                };

                getProductByIdResponseOut.ProductModules.Add(productModule);
            }
            else
            {
                productModule = new ProductModule()
                {
                    ModuleId = module.Id,
                    ModuleName = module.Name,
                    ModuleType = module.ModuleType,
                };

                getProductByIdResponseOut.ProductModules.Add(productModule);
            }

            foreach (var moduleService in module.ModuleServices)
            {
                var existProductModuleService = existproductmodule?.ProductModuleServices?.Where(x => x.ModuleServiceId == moduleService.Id).FirstOrDefault();
                var existmodule = getProductByIdResponseOut.ProductModules.Where(x => x.ModuleId == moduleService.ModuleId).First();

                if (existProductModuleService != null)
                {

                    existmodule.ProductModuleServices.Add(new ProductModuleService()
                    {
                        Id = existProductModuleService.Id,
                        IsActive = existProductModuleService.IsActive.HasValue ? existProductModuleService.IsActive.Value : false,
                        ModuleServiceId = existProductModuleService.ModuleServiceId,
                        ProductId = existProductModuleService.ProductId,
                        ProductModuleId = existProductModuleService.ProductModuleId,
                        ModuleServiceName = existProductModuleService.ModuleService.Name
                    });
                }
                else
                {
                    existmodule.ProductModuleServices.Add(new ProductModuleService()
                    {
                        ModuleServiceId = moduleService.Id,
                        ModuleServiceName = moduleService.Name,
                        ProductId = product != null ? product.Id : 0,
                    });
                }

            }
        }

        return _response.Success(new GetProductByIdOut("Get product successful.", getProductByIdResponseOut));
    }
}
