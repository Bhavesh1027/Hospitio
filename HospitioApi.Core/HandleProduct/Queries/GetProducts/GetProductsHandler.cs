using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleProduct.Queries.GetProducts;

public record GetProductsRequest(GetProductsIn In)
    : IRequest<AppHandlerResponse>;

public class GetProductsHandler : IRequestHandler<GetProductsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetProductsHandler(
         IDapperRepository dapper,
        IHandlerResponseFactory response
        )
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetProductsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("SearchValue", request.In.SearchValue, DbType.String);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("SortColumn", request.In.SortColumn, DbType.String);
        spParams.Add("SortOrder", request.In.SortOrder, DbType.String);


        var product = await _dapper.GetAll<GetProductsResponseOut>("[dbo].[GetProducts]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (product == null || product.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }


        return _response.Success(new GetProductsOut("Get products successful.", product));
    }
}
