using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleProduct.Commands.CreateProduct;

public record CreateProductRequest(CreateProductIn In)
    : IRequest<AppHandlerResponse>;

public class CreateProductHandler : IRequestHandler<CreateProductRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateProductHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        Product lastProduct;
        if (request.In.ProductId == 0)
        {
            lastProduct = await _db.Products.Where(s => s.Name == request.In.ProductName).FirstOrDefaultAsync(cancellationToken);
        }
        else
        {
            lastProduct = await _db.Products.Where(s => s.Name == request.In.ProductName && s.Id != request.In.ProductId).FirstOrDefaultAsync(cancellationToken);
        }
       

        if (lastProduct != null)
        {
            return _response.Error("product name is already exits", AppStatusCodeError.Forbidden403);
        }

        var product = new Product()
        {
            Id = request.In.ProductId,
            Name = request.In.ProductName,
            IsActive = request.In.IsActive
        };

        _db.Products.Update(product);

        await _db.SaveChangesAsync(cancellationToken);

        if (request.In.ProductModules != null)
        {
            var productkmodules = request.In.ProductModules.Select(pm => new ProductModule()
            {
                Id = pm.Id,
                Currency = pm.Currency,
                Module2TypeValue = pm.Module2TypeValue,
                IsActive = pm.IsActive,
                ModuleId = pm.ModuleId,
                Price = pm.Price.HasValue ? pm.Price.Value : 0,
                ProductId = product.Id,
                ProductModuleServices = pm.ProductModuleServices.Select(pms => new Data.Models.ProductModuleService()
                {
                    Id = pms.Id,
                    IsActive = pm.IsActive == false ? false : pms.IsActive,
                    ModuleServiceId = pms.ModuleServiceId,
                    ProductId = product.Id,
                    ProductModuleId = pms.ProductModuleId,
                }).ToList()
            });

            _db.ProductModules.UpdateRange(productkmodules);

            await _db.SaveChangesAsync(cancellationToken);
        }

        return _response.Success(new CreateProductOut("Save product successful.", new()
        {
            Id = product.Id,
            Name = product.Name
        }));
    }
}
