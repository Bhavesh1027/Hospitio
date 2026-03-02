using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservations;
public record GetCustomerReservationsRequest(GetCustomerReservationsIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerReservationsHandler : IRequestHandler<GetCustomerReservationsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerReservationsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerReservationsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.In.CustomerId, DbType.Int32);
        spParams.Add("SearchColumn", request.In.SearchColumn, DbType.String);
        spParams.Add("SearchValue", request.In.SearchValue, DbType.String);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("SortColumn", request.In.SortColumn, DbType.String);
        spParams.Add("SortOrder", request.In.SortOrder, DbType.String);

        var result = await _dapper
            .GetAll<CustomerReservationsOut>("[dbo].[GetCustomerReservations]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (result == null || result.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomerReservationsOut("Get customer reservations successful.", result));
    }
}
