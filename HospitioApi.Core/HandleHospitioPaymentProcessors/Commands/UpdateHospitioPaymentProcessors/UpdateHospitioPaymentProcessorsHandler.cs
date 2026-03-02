using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Commands.UpdateHospitioPaymentProcessors;
public record UpdateHospitioPaymentProcessorsRequest(UpdateHospitioPaymentProcessorsIn In) : IRequest<AppHandlerResponse>;
public class UpdateHospitioPaymentProcessorsHandler : IRequestHandler<UpdateHospitioPaymentProcessorsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public UpdateHospitioPaymentProcessorsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(UpdateHospitioPaymentProcessorsRequest request, CancellationToken cancellationToken)
    {
        var updateHospitioPaymentProcessor = await _db.HospitioPaymentProcessors.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (updateHospitioPaymentProcessor == null)
        {
            return _response.Error($"Hospitio payment processor with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        updateHospitioPaymentProcessor.PaymentProcessorId = request.In.PaymentProcessorId;
        //updateHospitioPaymentProcessor.ClientId = request.In.ClientId;
        //updateHospitioPaymentProcessor.ClientSecret = request.In.ClientSecret;
        //updateHospitioPaymentProcessor.Currency = request.In.Currency;

        await _db.SaveChangesAsync(cancellationToken);
        var updatedHospitioPaymentProcessorsOut = new UpdatedHospitioPaymentProcessorsOut()
        {
            Id = request.In.Id,
            PaymentProcessorId = request.In.PaymentProcessorId,
            ClientId = request.In.ClientId,
            ClientSecret = request.In.ClientSecret,
            Currency = request.In.Currency
        };
        return _response.Success(new UpdateHospitioPaymentProcessorsOut("Update hospitio payment processor successful.", updatedHospitioPaymentProcessorsOut));
    }
}
