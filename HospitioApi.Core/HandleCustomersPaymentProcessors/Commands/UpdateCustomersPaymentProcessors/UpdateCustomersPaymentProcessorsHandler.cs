using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.UpdateCustomersPaymentProcessors;
public record UpdateCustomersPaymentProcessorsRequest(UpdateCustomersPaymentProcessorsIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomersPaymentProcessorsHandler : IRequestHandler<UpdateCustomersPaymentProcessorsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdateCustomersPaymentProcessorsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(UpdateCustomersPaymentProcessorsRequest request, CancellationToken cancellationToken)
    {
        var updateCustomersPaymentProcessors = await _db.CustomerPaymentProcessors.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (updateCustomersPaymentProcessors == null)
        {
            return _response.Error($"Customers payment processors with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        //updateCustomersPaymentProcessors.ClientId = request.In.ClientId;
        updateCustomersPaymentProcessors.CustomerId = request.In.CustomerId;
        updateCustomersPaymentProcessors.PaymentProcessorId = request.In.PaymentProcessorId;
        //updateCustomersPaymentProcessors.ClientSecret = request.In.ClientSecret;
        //updateCustomersPaymentProcessors.Currency = request.In.Currency;

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdateCustomersPaymentProcessorsOut("Update customers payment processors successfull.", new()
        {
            Id = request.In.Id,
            CustomerId = request.In.CustomerId,
            ClientId = request.In.ClientId,
            PaymentProcessorId = request.In.PaymentProcessorId,
            ClientSecret = request.In.ClientSecret,
            Currency = request.In.Currency
        }));
    }
}
