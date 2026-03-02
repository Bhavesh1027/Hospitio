using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationByReservationNumber;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;
using System.Globalization;

namespace HospitioApi.Core.HandleCustomerReservation.Commands.ExtendCustomerGuestReservation;
public record ExtendCustomerGuestReservationRequest(ExtendCustomerGuestReservationIn In) : IRequest<AppHandlerResponse>;
public class ExtendCustomerGuestReservationHandler : IRequestHandler<ExtendCustomerGuestReservationRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    public ExtendCustomerGuestReservationHandler(ApplicationDbContext db, IHandlerResponseFactory response, IDapperRepository dapper)
    {
        _db = db;
        _response = response;
        _dapper = dapper;
    }
    public async Task<AppHandlerResponse> Handle(ExtendCustomerGuestReservationRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("ReservationNumber", request.In.ReservationNumber, DbType.String);
        var customerReservationByNumberOut = await _dapper
            .GetSingle<CustomerReservationByNumberOut>("[dbo].[GetCustomerReservationByNumber]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);
        if (customerReservationByNumberOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        var customerReservation = await _db.CustomerReservations.Where(e => e.Id == customerReservationByNumberOut.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerReservation == null)
        {
            return _response.Error($"Customer reservation could not be found.", AppStatusCodeError.Gone410);
        }
            
        string format = "dd/MM/yyyy HH:mm";
        string combinedCheckOutDateTime = $"{request.In.DepartureDate} {request.In.DepartureTime}";
        if (DateTime.TryParseExact(combinedCheckOutDateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime departureDateTime))
        {
            customerReservation.CheckoutDate = departureDateTime;
        }
        await _db.SaveChangesAsync(cancellationToken);

        var extendedCustomerReservationOut = new ExtendedCustomerReservationOut()
        {
            Id = customerReservationByNumberOut.Id
        };

        return _response.Success(new ExtendCustomerGuestReservationOut("Extend customer reservation successful.", extendedCustomerReservationOut));
    }
}
