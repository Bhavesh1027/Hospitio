using Dapper;
using MediatR;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestAppBuilderByCustomerRoomId;
using HospitioApi.Core.HandleDepartment.Queries.GetDepartments;
using HospitioApi.Core.Services.Chat.Models.Chat;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoByAppBuilderId;
public record GetCustomersPropertiesInfoByAppBuilderIdRequest(GetCustomersPropertiesInfoByAppBuilderIdIn In, string CustomerId,UserTypeEnum UserType) : IRequest<AppHandlerResponse>;

public class GetCustomersPropertiesInfoByAppBuilderIdHandler : IRequestHandler<GetCustomersPropertiesInfoByAppBuilderIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomersPropertiesInfoByAppBuilderIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomersPropertiesInfoByAppBuilderIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("AppBuilderId", request.In.AppBuilderId, DbType.Int32);
        if(UserTypeEnum.Customer == request.UserType)
        {
            spParams.Add("CustomerId", request.CustomerId, DbType.Int32);
        }
        else if(UserTypeEnum.Guest == request.UserType)
        {
            spParams.Add("CustomerId", request.In.CustomerId, DbType.Int32);
        }
        spParams.Add("UserType", (int)(UserTypeEnum)Enum.Parse(typeof(UserTypeEnum), request.UserType.ToString()), DbType.Int32);

        var getAppBuilder = await _dapper.GetAllJsonData<CustomersPropertiesInfoByAppBuilderIdOut>("[dbo].[AddPropertyInfo]", spParams, cancellationToken, CommandType.StoredProcedure);

        if (getAppBuilder == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomersPropertiesInfoByAppBuilderIdOut("Get customer guest property info successful.", getAppBuilder));
    }
}