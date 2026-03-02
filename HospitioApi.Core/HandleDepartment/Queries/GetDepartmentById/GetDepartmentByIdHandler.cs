using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleDepartment.Queries.GetDepartmentById;

public record GetDepartmentByIdRequest(int Id)
    : IRequest<AppHandlerResponse>;

public class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetDepartmentByIdHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response
        )
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetDepartmentByIdRequest request, CancellationToken cancellationToken)
    {

        var spParams = new DynamicParameters();
        spParams.Add("Id", request.Id, DbType.Int32);

        var getDepartmentByIdResponseOut = await _dapper.GetSingle<GetDepartmentByIdResponseOut>("[dbo].[GetDepartmentById]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (getDepartmentByIdResponseOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }



        return _response.Success(new GetDepartmentByIdOut("Get department successful.", getDepartmentByIdResponseOut));
    }
}
