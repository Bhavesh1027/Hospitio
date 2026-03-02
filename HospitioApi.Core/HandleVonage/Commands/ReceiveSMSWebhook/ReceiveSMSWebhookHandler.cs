using MediatR;
using HospitioApi.Core.HandleVonage.Commands.InboundWebhook;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleVonage.Commands.ReceiveSMSWebhook;

public record ReceiveSMSWebhookHandlerRequest(ReceiveSMSWebhookIn In) : IRequest<AppHandlerResponse>;

public class ReceiveSMSWebhookHandler
{
    private readonly IHandlerResponseFactory _response;
    public ReceiveSMSWebhookHandler(IHandlerResponseFactory response)
    {
        _response=response;
    }
    public async Task<AppHandlerResponse> Handle(ReceiveSMSWebhookHandlerRequest request, CancellationToken cancellationToken)
    {
        return _response.Success(new ReceiveSMSWebhookOut("Receive"));
    }
}

