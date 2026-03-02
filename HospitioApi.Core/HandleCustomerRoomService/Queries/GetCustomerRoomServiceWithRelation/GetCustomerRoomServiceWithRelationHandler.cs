using Dapper;
using MediatR;
using HospitioApi.Core.HandleCustomerRoomService.Queries.GetCustomerRoomServiceWithRelation;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerRoomService.Queries.GetCustomerEnhanceYourStayCategoryWithRelation;
public record GetCustomerRoomServiceWithRelationRequest(GetCustomerRoomServiceWithRelationIn In,string CustomerId, string UserType) : IRequest<AppHandlerResponse>;
public class GetCustomerRoomServiceWithRelationHandler : IRequestHandler<GetCustomerRoomServiceWithRelationRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerRoomServiceWithRelationHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerRoomServiceWithRelationRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("AppBuilderId", request.In.AppBuilderId, DbType.Int32);
        spParams.Add("UserType", request.UserType != null ? request.UserType : 3, DbType.Int32);
        //spParams.Add("CustomerId", request.CustomerId, DbType.Int32);

        var getCustomerRoomServiceWithRelationOuts = await _dapper.GetAllJsonData<CustomerRoomServiceWithRelationOut>("[dbo].[GetCustomerRoomServiceWithRelationSP]"
      , spParams, cancellationToken,
      commandType: CommandType.StoredProcedure);

        if(getCustomerRoomServiceWithRelationOuts == null || getCustomerRoomServiceWithRelationOuts.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerRoomServiceWithRelationOut("Get customer room service successful.", getCustomerRoomServiceWithRelationOuts));
    }
}


