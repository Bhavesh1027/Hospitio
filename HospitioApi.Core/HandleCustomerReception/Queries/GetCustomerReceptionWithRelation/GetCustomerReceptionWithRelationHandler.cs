using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerReception.Queries.GetCustomerReceptionWithRelation;
public record GetCustomerReceptionWithRelationRequest(GetCustomerReceptionWithRelationIn In,string CustomerId, string UserType) : IRequest<AppHandlerResponse>;
public class GetCustomerReceptionWithRelationHandler : IRequestHandler<GetCustomerReceptionWithRelationRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerReceptionWithRelationHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerReceptionWithRelationRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("AppBuilderId", request.In.AppBuilderId, DbType.Int32);
        spParams.Add("UserType", request.UserType != null ? request.UserType : 3, DbType.Int32);
        //spParams.Add("CustomerId", request.CustomerId, DbType.Int32);

        var getCustomerReceptionWithRelationOuts = await _dapper.GetAllJsonData<CustomerReceptionWithRelationOut>("[dbo].[GetCustomerReceptionWithRelationSP]"
      , spParams, cancellationToken,
      commandType: CommandType.StoredProcedure);
        if(getCustomerReceptionWithRelationOuts == null || getCustomerReceptionWithRelationOuts.Count ==0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerReceptionWithRelationOut("Get customer reception successful.", getCustomerReceptionWithRelationOuts));
    }
}


