using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleNotifications.Queries.GetNotifications;
public record GetNotificationsRequest(GetNotificationsIn In) : IRequest<AppHandlerResponse>;
public class GetNotificationsHandler : IRequestHandler<GetNotificationsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetNotificationsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetNotificationsRequest request, CancellationToken cancellationToken)
    {
     
        var spParams = new DynamicParameters();
        spParams.Add("UserId", request.In.UserId, DbType.Int32);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("UserType" , request.In.UserType, DbType.Int32);
        // SP Name is GetGroups

        var notification = await _dapper.GetAll<NotificationOut>("[dbo].[GetNotifications]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (notification == null || notification.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetNotificationsOut("Get notification successful.", notification));
    }
}
