using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestPortalCheckInFormBuilder;
public record GetCustomerGuestPortalCheckInFormBuilderRequest(GetCustomerGuestPortalCheckInFormBuilderIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerGuestPortalCheckInFormBuilderHandler : IRequestHandler<GetCustomerGuestPortalCheckInFormBuilderRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly ApplicationDbContext _db;

    public GetCustomerGuestPortalCheckInFormBuilderHandler(IDapperRepository dapper, IHandlerResponseFactory response, ApplicationDbContext db)
    {
        _dapper = dapper;
        _response = response;
        _db = db;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerGuestPortalCheckInFormBuilderRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("GuestId", request.In.GuestId, System.Data.DbType.Int32);
        spParams.Add("ReservationId", request.In.ReservationId, System.Data.DbType.Int32);

        var result = await _dapper
            .GetAllJsonData<GetCustomerGuestResponseOut>("[dbo].[GetCustomerReservationWithCustomerPropInfo]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (result == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        if (DateTime.UtcNow > result[0].GetCustomerReservationResponseOut.CheckoutDate.AddDays(10))
        {
            return _response.Error("Link is expired", AppStatusCodeError.Forbidden403);
        }

        var customerGuest = await _db.CustomerGuests.Where(g => g.Id == request.In.GuestId).FirstOrDefaultAsync(cancellationToken);
        if (customerGuest.RoomNumber == null)
        {
            result[0].BuidlerId = 1;
            result[0].RoomId = 1;
            result[0].RoomName = "101";
        }
        else
        {
            int? customerId = result[0].GetCustomerReservationResponseOut.CustomerId;
            var customerRoomName = await _db.CustomerRoomNames.Where(c => c.Name == customerGuest.RoomNumber && c.CustomerId == customerId).FirstOrDefaultAsync(cancellationToken);
            result[0].BuidlerId = await _db.CustomerGuestAppBuilders.Where(c => c.CustomerRoomNameId == customerRoomName.Id).Select(c => c.Id).FirstOrDefaultAsync(cancellationToken);
            result[0].RoomId = customerRoomName.Id;
            result[0].RoomName = customerRoomName.Name;
        }

        return _response.Success(new GetCustomerGuestPortalCheckInFormBuilderOut("Get customer reservation successful.", result[0]));
    }
}
