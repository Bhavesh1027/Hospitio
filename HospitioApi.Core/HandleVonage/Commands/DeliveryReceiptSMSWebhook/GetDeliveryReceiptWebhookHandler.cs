using MediatR;
using HospitioApi.Core.HandleVonage.Commands.InboundWebhook;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleVonage.Commands.DeliveryReceiptSMSWebhook;

public record GetDeliveryReceiptWebhookHandlerRequest(GetDeliveryReceiptWebhookIn In) : IRequest<AppHandlerResponse>;
public class GetDeliveryReceiptWebhookHandler : IRequestHandler<GetDeliveryReceiptWebhookHandlerRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    public GetDeliveryReceiptWebhookHandler(IHandlerResponseFactory response)
    {
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetDeliveryReceiptWebhookHandlerRequest request, CancellationToken cancellationToken)
    {
        return _response.Success(new GetDeliveryReceiptWebhookOut("Receive"));
    }
}

