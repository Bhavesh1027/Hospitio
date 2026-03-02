using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomerLatitudeLongitude;
public record GetCustomerLatitudeLongitudeRequest(GetCustomerLatitudeLongitudeIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerLatitudeLongitudeHandler : IRequestHandler<GetCustomerLatitudeLongitudeRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerLatitudeLongitudeHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerLatitudeLongitudeRequest request, CancellationToken cancellationToken)
    {
        var customerInfo = await _db.Customers.Include(c => c.BusinessType).Where(s => s.Id == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);
        var customerPropertyInfo = await _db.CustomerPropertyInformations.Where(s => s.CustomerGuestAppBuilderId == request.In.BuilderId && s.CustomerId == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);

        if (customerInfo == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        var customerLatitudeLongitude = new CustomerLatitudeLongitude
        {
            BusinessName = customerInfo.BusinessName,
            Latitude = (customerInfo?.BusinessType?.BizType == "Vacation Rental") ? customerPropertyInfo?.Latitude : customerInfo?.Latitude,
            Longitude = (customerInfo?.BusinessType?.BizType == "Vacation Rental") ? customerPropertyInfo?.Longitude : customerInfo?.Longitude
        };
        return _response.Success(new GetCustomerLatitudeLongitudeOut("Get Customer Latitude Longitude successful.", customerLatitudeLongitude));
    }
}
