using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleBusinessTypes.Queries.GetBusinessTypes;
public record GetBusinessTypesRequest() : IRequest<AppHandlerResponse>;
public class GetBusinessTypesHandler : IRequestHandler<GetBusinessTypesRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetBusinessTypesHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetBusinessTypesRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();


        // SP Name is [GetBusinessTypes]
        var businessType = await _dapper
            .GetAll<BusinessTypesOut>("[dbo].[GetBusinessTypes]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (businessType == null || businessType.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetBusinessTypesOut("Get business type successful.", businessType));

    }
}
