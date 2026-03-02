using Dapper;
using MediatR;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuests;
public record GetCustomerGuestsRequest(GetCustomerGuestsIn In, int CustomerId) : IRequest<AppHandlerResponse>;
public class GetCustomerGuestsHandler : IRequestHandler<GetCustomerGuestsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly FrontEndLinksSettingsOptions _frontEndLinksSettings;

    public GetCustomerGuestsHandler(IDapperRepository dapper, IHandlerResponseFactory response, IOptions<FrontEndLinksSettingsOptions> frontEndLinksSettings)
    {
        _dapper = dapper;
        _response = response;
        _frontEndLinksSettings = frontEndLinksSettings.Value;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerGuestsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.CustomerId, DbType.Int32);
        spParams.Add("SearchValue", request.In.SearchValue, DbType.String);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("SortColumn", request.In.SortColumn, DbType.String);
        spParams.Add("SortOrder", request.In.SortOrder, DbType.String);

        var result = await _dapper
            .GetAll<CustomerGuestsOut>("[dbo].[GetCustomerGuests]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        var updatedResult = result.ToList().Select(item => new CustomerGuestsOut{
                Id = item.Id,
                CustomerReservationId = item.CustomerReservationId,
                Firstname = item.Firstname, Lastname = item.Lastname,
                RoomNumber = item.RoomNumber,
                GuestStatus = item.GuestStatus,
                CheckinDate = item.CheckinDate,
                CheckoutDate = item.CheckoutDate,
                FilteredCount = item.FilteredCount,
                GuestToken = _frontEndLinksSettings.GuestPortal + "?id=" + item.GuestToken
        })
        .ToList();

        foreach (var item in result)
        {
            item.GuestToken = _frontEndLinksSettings.GuestPortal + "?id=" + item.GuestToken;
        }

        if (result == null || result.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomerGuestsOut("Get customer guests successful.", updatedResult));
    }
}
