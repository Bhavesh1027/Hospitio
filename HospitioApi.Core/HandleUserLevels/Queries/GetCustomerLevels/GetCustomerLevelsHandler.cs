using MediatR;
using HospitioApi.Shared.Enums;
using HospitioApi.Shared;
using HospitioApi.Core.HandleUserLevels.Queries.GetUserLevels;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using Dapper;
using System.Data;

namespace HospitioApi.Core.HandleUserLevels.Queries.GetCustomerLevels;

public record GetCustomerLevelsRequest(UserTypeEnum UserTypeEnum) : IRequest<AppHandlerResponse>;
public class GetCustomerLevelsHandler : IRequestHandler<GetCustomerLevelsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerLevelsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerLevelsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        if (request.UserTypeEnum == UserTypeEnum.Customer)
        {
            spParams.Add("IsCustomerUserType", request.UserTypeEnum, DbType.Boolean);
        }

        var customerLevel = await _dapper.GetAll<CustomerLevelsOut>("[dbo].[GetCustomerLevels]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customerLevel == null || customerLevel.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerLevelsOut("Get customer levels successful.", customerLevel));
    }
}
