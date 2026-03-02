using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.CreateCustomersPaymentProcessors;
public record CreateCustomersPaymentProcessorsRequest(CreateCustomersPaymentProcessorsIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomersPaymentProcessorsHandler : IRequestHandler<CreateCustomersPaymentProcessorsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateCustomersPaymentProcessorsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomersPaymentProcessorsRequest request, CancellationToken cancellationToken)
    {
        var customersPaymentProcessor = new CustomerPaymentProcessor
        {
            CustomerId = request.In.CustomerId,
            PaymentProcessorId = request.In.PaymentProcessorId,
            //ClientId = request.In.ClientId,
            //ClientSecret = request.In.ClientSecret,
            //Currency = request.In.Currency
        };

        await _db.CustomerPaymentProcessors.AddAsync(customersPaymentProcessor, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        var createCustomersPaymentProcessorsOut = new CreatedCustomersPaymentProcessorsOut
        {
            Id = customersPaymentProcessor.Id,
            CustomerId = customersPaymentProcessor.CustomerId,
            PaymentProcessorId = customersPaymentProcessor.PaymentProcessorId
        };

        return _response.Success(new CreateCustomersPaymentProcessorsOut("Create customer payment processor successful.", createCustomersPaymentProcessorsOut));
    }
}
