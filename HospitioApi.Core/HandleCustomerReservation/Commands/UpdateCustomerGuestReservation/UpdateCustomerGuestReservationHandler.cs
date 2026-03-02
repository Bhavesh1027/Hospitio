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

namespace HospitioApi.Core.HandleCustomerReservation.Commands.UpdateCustomerGuestReservation;
public record UpdateCustomerGuestReservationRequest(UpdateCustomerGuestReservationIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomerGuestReservationHandler: IRequestHandler<UpdateCustomerGuestReservationRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    public UpdateCustomerGuestReservationHandler(ApplicationDbContext db, IHandlerResponseFactory response, IDapperRepository dapper)
    {
        _db = db;
        _response = response;
        _dapper = dapper;
    }
    public async Task<AppHandlerResponse> Handle(UpdateCustomerGuestReservationRequest request, CancellationToken cancellationToken)
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

        customerGuest.Email = request.In.Email;
        customerGuest.Firstname = request.In.FirstName;
        customerGuest.Lastname = request.In.LastName;
        customerGuest.PhoneNumber = request.In.PhoneNumber;
        await _db.SaveChangesAsync(cancellationToken);

        var updateCustomerGuestAlertsOut = new UpdatedCustomerReservationOut()
        {
            Id = customerReservationByNumberOut.Id
        };

        return _response.Success(new UpdateCustomerGuestReservationOut("Update customer reservation successful.", updateCustomerGuestAlertsOut));
    }
}
