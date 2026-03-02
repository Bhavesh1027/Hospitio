using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleDepartment.Queries.GetDepartments;

public record GetDepartmentsRequest(GetDepartmentsIn In)
    : IRequest<AppHandlerResponse>;

public class GetDepartmentsHandler : IRequestHandler<GetDepartmentsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetDepartmentsHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response
        )
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetDepartmentsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("UserType", request.In.UserType, DbType.Int32);
        spParams.Add("UserId" , request.In.UserId, DbType.Int32);

        var departments = await _dapper.GetAll<GetDepartmentsResponseOut>("[dbo].[GetDepartments]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (departments == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetDepartmentsOut("Get departments successful.", departments));
    }
}
