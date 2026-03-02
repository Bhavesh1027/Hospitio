using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandlePaymentProcessors.Commands.CreatePaymentProcessors;
public record CreatePaymentProcessorsRequest(CreatePaymentProcessorsIn In) : IRequest<AppHandlerResponse>;
public class CreatePaymentProcessorsHandler : IRequestHandler<CreatePaymentProcessorsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public CreatePaymentProcessorsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreatePaymentProcessorsRequest request, CancellationToken cancellationToken)
    {
        //var ckeckExist = await _db.PaymentProcessors.Where(e => e.Name == request.In.Name).FirstOrDefaultAsync(cancellationToken);
        //if (ckeckExist != null)
        //{
        //    return _response.Error($"The Payment Processor {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
        //}
        var paymentProcessors = new PaymentProcessor
        {
            //Name = request.In.Name,
            IsActive = true
        };

        await _db.PaymentProcessors.AddAsync(paymentProcessors, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        var createpaymentProcessor = new CreatedPaymentProcessorsOut
        {
            Id = paymentProcessors.Id,
            //Name = paymentProcessors.Name,
        };

        return _response.Success(new CreatePaymentProcessorsOut("Create payment processor successful.", createpaymentProcessor));
    }
}
