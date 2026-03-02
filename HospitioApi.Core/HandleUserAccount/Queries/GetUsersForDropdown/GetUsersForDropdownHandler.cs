using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleUserAccount.Queries.GetUsersForDropdown;
public record GetUsersForDropdownRequest(int userId) : IRequest<AppHandlerResponse>;
public class GetUsersForDropdownHandler : IRequestHandler<GetUsersForDropdownRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly ApplicationDbContext _db;

    public GetUsersForDropdownHandler(IDapperRepository dapper, IHandlerResponseFactory response, ApplicationDbContext applicationDbContext)
    {
        _dapper = dapper;
        _response = response;
        _db = applicationDbContext;
    }

    public async Task<AppHandlerResponse> Handle(GetUsersForDropdownRequest request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.Where(u => u.Id == request.userId).FirstOrDefaultAsync(cancellationToken);

        var spParams = new DynamicParameters();

        spParams.Add("UserId", user.Id, DbType.Int32);
        spParams.Add("UserLevel", user.UserLevelId, DbType.Int32);

        var usersOut = await _dapper
            .GetAll<AdminUsersOut>("[dbo].[GetAdminUsers]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (usersOut == null || usersOut.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetUsersForDropdownOut("Get admin staff successful.", usersOut));
    }
}
