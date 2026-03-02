using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Commands.CreateHospitioPaymentProcessors;
public record CreateHospitioPaymentProcessorsRequest(CreateHospitioPaymentProcessorsIn In) : IRequest<AppHandlerResponse>;
public class CreateHospitioPaymentProcessorsHandler : IRequestHandler<CreateHospitioPaymentProcessorsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateHospitioPaymentProcessorsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(CreateHospitioPaymentProcessorsRequest request, CancellationToken cancellationToken)
    {
        var hospitioPaymentProcessors = new HospitioPaymentProcessor
        {
            PaymentProcessorId = request.In.PaymentProcessorId,
            //ClientId = request.In.ClientId,
            //ClientSecret = request.In.ClientSecret,
            //Currency = request.In.Currency
        };

        await _db.HospitioPaymentProcessors.AddAsync(hospitioPaymentProcessors, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        var createrdefypaymentprocessor = new CreatedHospitioPaymentProcessorsOut
        {
            Id = hospitioPaymentProcessors.Id,
            PaymentProcessorId = hospitioPaymentProcessors.PaymentProcessorId,
            //ClientId = hospitioPaymentProcessors.ClientId,
            //ClientSecret = hospitioPaymentProcessors.ClientSecret,
            //Currency = hospitioPaymentProcessors.Currency
        };

        return _response.Success(new CreateHospitioPaymentProcessorsOut("Create hospitio payment processor successful.", createrdefypaymentprocessor));
    }
}
