using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandlePaymentProcessors.Commands.UpdatePaymentProcessors;
public record UpdatePaymentProcessorsRequest(UpdatePaymentProcessorsIn In) : IRequest<AppHandlerResponse>;
public class UpdatePaymentProcessorsHandler : IRequestHandler<UpdatePaymentProcessorsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdatePaymentProcessorsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdatePaymentProcessorsRequest request, CancellationToken cancellationToken)
    {
        var paymentProcessors = await _db.PaymentProcessors.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (paymentProcessors == null)
        {
            return _response.Error($"Payment processor with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        paymentProcessors.GRName = request.In.Name;
        _db.PaymentProcessors.Update(paymentProcessors);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdatePaymentProcessorsOut("Update payment processors successful.", new()
        {
            Id = request.In.Id,
            Name = request.In.Name
        }));

    }
}
