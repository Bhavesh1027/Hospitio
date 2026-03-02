using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleCustomerGuest.Queries.GetMainGuestByReservationId;
using HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationByReservationNumber;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;
using System.Globalization;

namespace HospitioApi.Core.HandleCustomerReservation.Commands.TransferCustomerGuestReservation;
public record TransferCustomerGuestReservationRequest(TransferCustomerGuestReservationIn In) : IRequest<AppHandlerResponse>;
public class TransferCustomerGuestReservationHandler: IRequestHandler<TransferCustomerGuestReservationRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    public TransferCustomerGuestReservationHandler(ApplicationDbContext db, IHandlerResponseFactory response, IDapperRepository dapper)
    {
        _db = db;
        _response = response;
        _dapper = dapper;
    }
    public async Task<AppHandlerResponse> Handle(TransferCustomerGuestReservationRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("ReservationNumber", request.In.ReservationNumber, DbType.String);
        var customerReservationByNumberOut = await _dapper
            .GetSingle<CustomerReservationByNumberOut>("[dbo].[GetCustomerReservationByNumber]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);
        if (customerReservationByNumberOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        spParams = new DynamicParameters();
        spParams.Add("ReservationId", customerReservationByNumberOut.Id, DbType.Int32);
        var customerGuestByIdOut = await _dapper
            .GetSingle<CustomerGuestByReservationIdOut>("[dbo].[GetMainCustomerGuestByReservationId]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);
        if (customerGuestByIdOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        var customerGuest = await _db.CustomerGuests.Where(e => e.Id == customerGuestByIdOut.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerGuest == null)
        {
            return _response.Error($"Customer guest could not be found.", AppStatusCodeError.Gone410);
        }

        var customerRooms = await _db.CustomerRoomNames.Where(e => e.Guid == request.In.NewLocationCode).FirstOrDefaultAsync(cancellationToken);
        customerGuest.CustomerRoomGuid = request.In.NewLocationCode;
        customerGuest.RoomNumber = customerRooms.Name;

        var customerReservation = await _db.CustomerReservations.Where(e => e.Id == customerReservationByNumberOut.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerReservation == null)
        {
            return _response.Error($"Customer reservation could not be found.", AppStatusCodeError.Gone410);
        }

        string format = "dd/MM/yyyy HH:mm";
        string combinedCheckInDateTime = $"{request.In.ArrivalDate} {request.In.ArrivalTime}";
        string combinedCheckOutDateTime = $"{request.In.DepartureDate} {request.In.DepartureTime}";
        if (DateTime.TryParseExact(combinedCheckInDateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime arrivalDateTime))
        {
            customerReservation.CheckinDate = arrivalDateTime;
        }
        if (DateTime.TryParseExact(combinedCheckOutDateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime departureDateTime))
        {
            customerReservation.CheckoutDate = departureDateTime;
        }
        await _db.SaveChangesAsync(cancellationToken);

        var transferedCustomerReservationOut = new TransferedCustomerReservationOut()
        {
            Id = customerReservationByNumberOut.Id
        };

        return _response.Success(new TransferCustomerGuestReservationOut("Transfer customer reservation successful.", transferedCustomerReservationOut));
    }
}
