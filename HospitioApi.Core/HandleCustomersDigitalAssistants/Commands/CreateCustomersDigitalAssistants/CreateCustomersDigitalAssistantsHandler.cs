using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.CreateCustomersDigitalAssistants;
public record CreateCustomersDigitalAssistantsRequest(CreateCustomersDigitalAssistantsIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomersDigitalAssistantsHandler : IRequestHandler<CreateCustomersDigitalAssistantsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateCustomersDigitalAssistantsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomersDigitalAssistantsRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.CustomerDigitalAssistants.Where(e => e.Name == request.In.Name && e.CustomerId == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The digital assistant {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
        }

        var customersDigitalAssistant = new CustomerDigitalAssistant
        {
            CustomerId = request.In.CustomerId,
            Name = request.In.Name,
            Details = request.In.Details,
            Icon = request.In.Icon,
            IsActive = true
        };

        await _db.CustomerDigitalAssistants.AddAsync(customersDigitalAssistant, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        var createCustomersDigitalAssistantsOut = new CreatedCustomersDigitalAssistantsOut
        {
            Id = customersDigitalAssistant.Id,
            CustomerId = customersDigitalAssistant.CustomerId,
            Name = customersDigitalAssistant.Name,
            Details = customersDigitalAssistant.Details,
            Icon = customersDigitalAssistant.Icon
        };

        return _response.Success(new CreateCustomersDigitalAssistantsOut("Create customer digital assistant successful.", createCustomersDigitalAssistantsOut));
    }

}
