using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoById;
public record GetCustomersPropertiesInfoByIdRequest(GetCustomersPropertiesInfoByIdIn In,string UserType) : IRequest<AppHandlerResponse>;
public class GetCustomersPropertiesInfoByIdHandler : IRequestHandler<GetCustomersPropertiesInfoByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomersPropertiesInfoByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomersPropertiesInfoByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);
        spParams.Add("UserType", request.UserType, DbType.Int32);

        var result = await _dapper.GetSingle<CustomersPropertiesInfoByIdOut>("[dbo].[GetCustomersPropertiesInfoById]"
     , spParams, cancellationToken,
     commandType: CommandType.StoredProcedure);

        if (result == null)
        {
            return _response.Error("Customers propery info could not be found", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomersPropertiesInfoByIdOut("Get Customers Property Info successful.", result));
    }
}
