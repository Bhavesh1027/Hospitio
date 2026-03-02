using MediatR;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.CreateCustomersPropertiesInfo;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;    
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Text.Json.Nodes;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.UpdateCustomersPropertiesInfo;
public record UpdateCustomersPropertiesInfoRequest(UpdateCustomersPropertiesInfoIn In, string CustomerId) : IRequest<AppHandlerResponse>;
public class UpdateCustomersPropertiesInfoHandler : IRequestHandler<UpdateCustomersPropertiesInfoRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdateCustomersPropertiesInfoHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(UpdateCustomersPropertiesInfoRequest request, CancellationToken cancellationToken)
    {
        if (request.In.Id > 0)
        {
            var updateCustomersPropertiesInfo = await _db.CustomerPropertyInformations.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);

            if (updateCustomersPropertiesInfo == null)
            {
                return _response.Error($"Customer property info with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
            }

            //updateCustomersPropertiesInfo.CustomerId = Convert.ToInt32(request.CustomerId);
            ////updateCustomersPropertiesInfo.CustomerGuestAppBuilderId = request.In.CustomerGuestAppBuilderId;
            //if (request.In.WifiUsername != null)
            //    updateCustomersPropertiesInfo.WifiUsername = request.In.WifiUsername;
            //if (request.In.WifiPassword != null)
            //    updateCustomersPropertiesInfo.WifiPassword = request.In.WifiPassword;
            //if (request.In.Overview != null)
            //    updateCustomersPropertiesInfo.Overview = request.In.Overview;
            //if (request.In.CheckInPolicy != null)
            //    updateCustomersPropertiesInfo.CheckInPolicy = request.In.CheckInPolicy;
            //if (request.In.TermsAndConditions != null)
            //    updateCustomersPropertiesInfo.TermsAndConditions = request.In.TermsAndConditions;
            //if (request.In.Street != null)
            //    updateCustomersPropertiesInfo.Street = request.In.Street;
            //if (request.In.StreetNumber != null)
            //    updateCustomersPropertiesInfo.StreetNumber = request.In.StreetNumber;
            //if (request.In.City != null)
            //    updateCustomersPropertiesInfo.City = request.In.City;
            //if (request.In.Postalcode != null)
            //    updateCustomersPropertiesInfo.Postalcode = request.In.Postalcode;
            //if (request.In.Country != null)
            //    updateCustomersPropertiesInfo.Country = request.In.Country;

            //updateCustomersPropertiesInfo.IsActive = true;
            updateCustomersPropertiesInfo.IsPublish = updateCustomersPropertiesInfo.IsPublish;
            updateCustomersPropertiesInfo.JsonData = JsonConvert.SerializeObject(request.In);

            await _db.SaveChangesAsync(cancellationToken);

            return _response.Success(new UpdateCustomersPropertiesInfoOut("Update customer property info Successful", new()
            {
                Id = request.In.Id,
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
                Latitude =request.In.Latitude,
                Longitude =request.In.Longitude
            }));
        }
        else
        {
            var customer = await _db.CustomerPropertyInformations.Include(i => i.Customer).Where(x => x.CustomerId == Convert.ToInt32(request.CustomerId) && x.IsActive == true).FirstOrDefaultAsync(cancellationToken);

            if (customer != null)
            {
                return _response.Error($"{customer.Customer?.BusinessName} is already exists in customer property information.", AppStatusCodeError.Forbidden403);
            }

            var customersPropertyInfo = new CustomerPropertyInformation
            {
                CustomerId = Convert.ToInt32(request.CustomerId),
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
                Latitude = request.In.Latitude,
                Longitude =request.In.Longitude,
                IsActive = true,
                IsPublish = false,
                JsonData = null
            };

            await _db.CustomerPropertyInformations.AddAsync(customersPropertyInfo, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);


            var createCustomersProprtiesInfoOut = new UpdatedCustomersPropertiesInfoOut
            {
                Id = customersPropertyInfo.Id,
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
                Latitude = request.In.Latitude,
                Longitude = request.In.Longitude
            };
            return _response.Success(new UpdateCustomersPropertiesInfoOut("Create customer property info successful.", createCustomersProprtiesInfoOut));
        }
    }
}
