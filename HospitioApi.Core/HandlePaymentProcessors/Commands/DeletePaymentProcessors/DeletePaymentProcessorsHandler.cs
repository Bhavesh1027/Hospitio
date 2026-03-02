using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandlePaymentProcessors.Commands.DeletePaymentProcessors;
public record DeletePaymentProcessorsRequest(DeletePaymentProcessorsIn In) : IRequest<AppHandlerResponse>;
public class DeletePaymentProcessorsHandler : IRequestHandler<DeletePaymentProcessorsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public DeletePaymentProcessorsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(DeletePaymentProcessorsRequest request, CancellationToken cancellationToken)
    {
        var paymentProcessor = await _db.PaymentProcessors.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);

        if (paymentProcessor == null)
        {
            return _response.Error($"Payment proccessor with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        _db.PaymentProcessors.Remove(paymentProcessor);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeletePaymentProcessorsOut("Delete payment processors successful", new()
        {
            Id = request.In.Id,
        }));
    }
}
