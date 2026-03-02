using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.CreateCustomerPropertyEmergencyNumber;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.UpdateCustomerPropertyEmergencyNumber;
public record UpdateCustomerPropertyEmergencyNumberRequest(UpdateCustomerPropertyEmergencyNumberIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomerPropertyEmergencyNumberHandler : IRequestHandler<UpdateCustomerPropertyEmergencyNumberRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public UpdateCustomerPropertyEmergencyNumberHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(UpdateCustomerPropertyEmergencyNumberRequest request, CancellationToken cancellationToken)
    {
        var item = request.In;
        if (item.Id > 0)
        {
            var customerPropertyEmergencyNumber = await _db.CustomerPropertyEmergencyNumbers.Where(e => e.Id == item.Id).FirstOrDefaultAsync(cancellationToken);
            if (customerPropertyEmergencyNumber == null)
            {
                return _response.Error($"Customer property emergency number with Id {item.Id} could not be found.", AppStatusCodeError.Gone410);
            }

            //customerPropertyEmergencyNumber.Name = item.Name;
            //customerPropertyEmergencyNumber.CustomerPropertyInformationId = item.CustomerPropertyInformationId;
            //customerPropertyEmergencyNumber.PhoneCountry = item.PhoneCountry;
            //customerPropertyEmergencyNumber.PhoneNumber = item.PhoneNumber;
            //customerPropertyEmergencyNumber.IsActive = true;
            customerPropertyEmergencyNumber.IsPublish = customerPropertyEmergencyNumber.IsPublish;
            customerPropertyEmergencyNumber.JsonData = JsonConvert.SerializeObject(item);
            //customerPropertyEmergencyNumber.DisplayOrder = item.DisplayOrder;

            await _db.SaveChangesAsync(cancellationToken);

            return _response.Success(new UpdateCustomerPropertyEmergencyNumberOut("Update customer property emergency number successful.", new()
            {
                Id = item.Id,
                Name = item.Name,
                PhoneCountry = item.PhoneCountry,
                PhoneNumber = item.PhoneNumber,
                CustomerPropertyInformationId = item.CustomerPropertyInformationId,
                DisplayOrder = customerPropertyEmergencyNumber.DisplayOrder
            }));
        }
        else
        {
            var customerPropertyEmergencyNumber = new CustomerPropertyEmergencyNumber()
            {
                CustomerPropertyInformationId = item.CustomerPropertyInformationId,
                Name = item.Name,
                PhoneCountry = item.PhoneCountry,
                PhoneNumber = item.PhoneNumber,
                IsActive = true,
                IsPublish = false,
                JsonData = null,
                DisplayOrder = item.DisplayOrder

            };

            await _db.CustomerPropertyEmergencyNumbers.AddAsync(customerPropertyEmergencyNumber, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return _response.Success(new UpdateCustomerPropertyEmergencyNumberOut("Create customer property emergency number successful.", new()
            {
                Id = customerPropertyEmergencyNumber.Id,
                CustomerPropertyInformationId = customerPropertyEmergencyNumber.CustomerPropertyInformationId,
                Name = customerPropertyEmergencyNumber.Name,
                PhoneCountry = customerPropertyEmergencyNumber.PhoneCountry,
                PhoneNumber = customerPropertyEmergencyNumber.PhoneNumber,
                DisplayOrder = customerPropertyEmergencyNumber.DisplayOrder
            }));
        }
    }
}
