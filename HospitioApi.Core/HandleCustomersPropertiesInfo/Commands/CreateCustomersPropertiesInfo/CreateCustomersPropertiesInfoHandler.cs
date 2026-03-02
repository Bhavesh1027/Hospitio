using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.CreateCustomersPropertiesInfo;
public record CreateCustomersPropertiesInfoRequest(CreateCustomersPropertiesInfoIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomersPropertiesInfoHandler : IRequestHandler<CreateCustomersPropertiesInfoRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateCustomersPropertiesInfoHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomersPropertiesInfoRequest request, CancellationToken cancellationToken)
    {
        var customersPropertyInfo = new CustomerPropertyInformation
        {
            CustomerId = request.In.CustomerId,
            CustomerGuestAppBuilderId = request.In.CustomerGuestAppBuilderId,
            WifiUsername = request.In.WifiUsername,
            WifiPassword = request.In.WifiPassword,
            Overview = request.In.Overview,
            CheckInPolicy = request.In.CheckInPolicy,
            TermsAndConditions = request.In.TermsAndConditions,
            Street = request.In.Street,
            StreetNumber = request.In.StreetNumber,
            City = request.In.City,
            Postalcode = request.In.Postalcode,
            Country = request.In.Country,
            IsActive = request.In.IsActive
        };

        await _db.CustomerPropertyInformations.AddAsync(customersPropertyInfo, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);


        var createCustomersProprtiesInfoOut = new CreatedCustomersPropertiesInfoOut
        {
            Id = customersPropertyInfo.Id,
            CustomerId = customersPropertyInfo.CustomerId,
            CustomerGuestAppBuilderId = customersPropertyInfo.CustomerGuestAppBuilderId,
            WifiUsername = customersPropertyInfo.WifiUsername,
            WifiPassword = customersPropertyInfo.WifiPassword,
            Overview = customersPropertyInfo.Overview,
            CheckInPolicy = customersPropertyInfo.CheckInPolicy,
            TermsAndConditions = customersPropertyInfo.TermsAndConditions,
            Street = customersPropertyInfo.Street,
            StreetNumber = customersPropertyInfo.StreetNumber,
            City = customersPropertyInfo.City,
            Postalcode = customersPropertyInfo.Postalcode,
            Country = customersPropertyInfo.Country,
            IsActive = customersPropertyInfo.IsActive
        };

        return _response.Success(new CreateCustomersPropertiesInfoOut("Create customer property info successful.", createCustomersProprtiesInfoOut));
    }
}
