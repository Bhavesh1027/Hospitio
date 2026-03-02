using Dapper;
using MediatR;
using HospitioApi.Shared;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandlerCustomerCommunication.Queries.GetCustomerCommunication;
public record GetCustomerCommunicationRequest(GetCustomerCommunicationIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerCommunicationHandler : IRequestHandler<GetCustomerCommunicationRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerCommunicationHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerCommunicationRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.In.CustomerId, System.Data.DbType.Int32);
        spParams.Add("SearchString", request.In.SearchString, System.Data.DbType.String);
        spParams.Add("PageNo", request.In.PageNo, System.Data.DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, System.Data.DbType.Int32);
        spParams.Add("CustomerUserLevel" , request.In.UserLevel , System.Data.DbType.String);
        spParams.Add("CustomerUserId" , request.In.CustomerUserId , System.Data.DbType.Int32);

        var customerLists = await _dapper
            .GetAll<CustomerCommunicationOut>("[dbo].[SP_Get_Communication_CustomerGuest]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (customerLists == null || customerLists.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerCommunicationOut("Get customers lists successfully.", customerLists));
    }
}
