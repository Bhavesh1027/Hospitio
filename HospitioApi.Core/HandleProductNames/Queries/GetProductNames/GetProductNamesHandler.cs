using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleProductNames.Queries.GetProductNames;
public record GetProductNamesRequest() : IRequest<AppHandlerResponse>;
public class GetProductNamesHandler : IRequestHandler<GetProductNamesRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetProductNamesHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetProductNamesRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();

        var productNames = await _dapper
            .GetAll<ProductNamesOut>("[dbo].[GetProductNames]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (productNames == null || productNames.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetProductNamesOut("Get product names successful.", productNames));

    }
}
