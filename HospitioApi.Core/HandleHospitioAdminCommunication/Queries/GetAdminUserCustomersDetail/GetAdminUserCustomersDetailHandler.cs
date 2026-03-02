using Dapper;
using MediatR;
using HospitioApi.Shared;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleHospitioAdminCommunication.Queries.GetAdminUserCustomersDetail;
public record GetAdminUserCustomersDetailRequest(GetAdminUserCustomersDetailIn In) : IRequest<AppHandlerResponse>;
public class GetAdminUserCustomersDetailHandler : IRequestHandler<GetAdminUserCustomersDetailRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetAdminUserCustomersDetailHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetAdminUserCustomersDetailRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.In.CustomerId, System.Data.DbType.Int32);
        spParams.Add("UserType", request.In.UserType, System.Data.DbType.String);

        var customerDetailLists = await _dapper
            .GetSingle<GetAdminUserCustomersDetailResponseOut>("[dbo].[SP_Get_AdminUserCustomersDetail]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (customerDetailLists == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetAdminUserCustomersDetailOut("Get admin user customers detail successfully.", customerDetailLists));
    }
}
