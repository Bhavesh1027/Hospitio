using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Migrations;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleVonage.Commands.StatusWebook;
public record GetStatusWebhookHandlerRequest(GetStatusWebhookIn In) : IRequest<AppHandlerResponse>;
public class GetStatusWebhookHandler : IRequestHandler<GetStatusWebhookHandlerRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly ApplicationDbContext _db;
    public GetStatusWebhookHandler(ApplicationDbContext db,IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetStatusWebhookHandlerRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Status Received");
        Console.WriteLine($"Message uuid: {request.In.message_uuid}");
        Console.WriteLine($"Status: {request.In.status}");
        //Console.WriteLine($"Error title: {request.In.error}");
        if (request.In.message_uuid != null)
        {
            var message = await _db.ChannelMessages.Where(e => e.MessageUuid == request.In.message_uuid).FirstOrDefaultAsync();
            if(message != null )
            {
                message.VonageStatus = request.In.status;
                _db.SaveChangesAsync(CancellationToken.None);
            } else
            {
                Console.WriteLine("not have message in our database");
            }
        }
        //Console.WriteLine($"Error detail: {request.In.error.detail}");

        return _response.Success(new GetStatusWebhookOut("Receive"));
    }
}

