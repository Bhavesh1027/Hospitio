using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Commands.DeleteHospitioPaymentProcessors;
public record DeleteHospitioPaymentProcessorsRequest(DeleteHospitioPaymentProcessorsIn In) : IRequest<AppHandlerResponse>;
public class DeleteHospitioPaymentProcessorsHandler : IRequestHandler<DeleteHospitioPaymentProcessorsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteHospitioPaymentProcessorsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteHospitioPaymentProcessorsRequest request, CancellationToken cancellationToken)
    {
        var hospitioPaymentProcessor = await _db.HospitioPaymentProcessors.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (hospitioPaymentProcessor == null)
        {
            return _response.Error($"Hospitio payment processor with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        _db.HospitioPaymentProcessors.Remove(hospitioPaymentProcessor);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteHospitioPaymentProcessorsOut("Delete hospitio payment processor successful.", new() { Id = request.In.Id }));
    }
}
