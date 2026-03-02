using Dapper;
using MediatR;
using HospitioApi.Core.HandleDepartment.Queries.GetDepartmentById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleDepartment.Queries.GetCustomerDepartmentById.cs;

public record GetCustomerDepartmentByIdRequest(int Id)
    : IRequest<AppHandlerResponse>;
public class GetCustomerDepartmentByIdHandler : IRequestHandler<GetCustomerDepartmentByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerDepartmentByIdHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response
        )
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerDepartmentByIdRequest request, CancellationToken cancellationToken)
    {

        var spParams = new DynamicParameters();
        spParams.Add("Id", request.Id, DbType.Int32);

        var getCustomerDepartmentByIdResponseOut = await _dapper.GetSingle<GetCustomerDepartmentByIdResponseOut>("[dbo].[GetCustomerDepartmentById]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (getCustomerDepartmentByIdResponseOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerDepartmentByIdOut("Get department successful.", getCustomerDepartmentByIdResponseOut));
    }
}
