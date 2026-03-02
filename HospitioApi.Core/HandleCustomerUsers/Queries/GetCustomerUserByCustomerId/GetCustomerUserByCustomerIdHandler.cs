using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerUserByCustomerId;
public record GetCustomerStaffsByCustomerIdRequest(string CustomerId, int UserId) : IRequest<AppHandlerResponse>;
public class GetCustomerUserByCustomerIdHandler : IRequestHandler<GetCustomerStaffsByCustomerIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly ApplicationDbContext _db;

    public GetCustomerUserByCustomerIdHandler(IDapperRepository dapper, IHandlerResponseFactory response, ApplicationDbContext applicationDbContext)
    {
        _dapper = dapper;
        _response = response;
        _db = applicationDbContext;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerStaffsByCustomerIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _db.CustomerUsers.Where(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.CustomerId, DbType.Int32);
        spParams.Add("UserId", request.UserId, DbType.Int32);
        spParams.Add("UserLevel", user.CustomerLevelId, DbType.Int32);

        //SP Name is GetCustomerStaffAlertsByCustomerId
        var customerStaffsOut = await _dapper
            .GetAll<CustomerUsersByCustomerIdOut>("[dbo].[GetCustomerUsersByCustomerId]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customerStaffsOut == null || customerStaffsOut.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomerUserByCustomerIdOut("Get customer staff successful.", customerStaffsOut));
    }
}
