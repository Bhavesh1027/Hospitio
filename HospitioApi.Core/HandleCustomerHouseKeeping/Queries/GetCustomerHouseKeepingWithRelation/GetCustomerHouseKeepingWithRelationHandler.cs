using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Queries.GetCustomerHouseKeepingWithRelation;
public record GetCustomerHouseKeepingWithRelationRequest(GetCustomerHouseKeepingWithRelationIn In,string CustomerId, string UserType) : IRequest<AppHandlerResponse>;
public class GetCustomerHouseKeepingWithRelationHandler : IRequestHandler<GetCustomerHouseKeepingWithRelationRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerHouseKeepingWithRelationHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerHouseKeepingWithRelationRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("AppBuilderId", request.In.AppBuilderId, DbType.Int32);
        spParams.Add("UserType", request.UserType != null ? request.UserType : 3, DbType.Int32);
        //spParams.Add("CustomerId", request.CustomerId, DbType.Int32);

        var getCustomerHouseKeepingWithRelationOuts = await _dapper.GetAllJsonData<CustomerHouseKeepingWithRelationOut>("[dbo].[GetCustomerHouseKeepingWithRelationSP]"
      , spParams, cancellationToken,
      commandType: CommandType.StoredProcedure);

        if(getCustomerHouseKeepingWithRelationOuts == null || getCustomerHouseKeepingWithRelationOuts.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerHouseKeepingWithRelationOut("Get customer house keeping successful.", getCustomerHouseKeepingWithRelationOuts));
    }
}


