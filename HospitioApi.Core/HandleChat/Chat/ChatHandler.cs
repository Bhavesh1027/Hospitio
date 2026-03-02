using MediatR;
using Microsoft.AspNetCore.SignalR;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleChat.Chat;
public record ChatRequest : IRequest<AppHandlerResponse>; 

public class ChatHandler : IRequestHandler<ChatRequest,AppHandlerResponse>
{
    private readonly ChatHub _hubContext;
    public ChatHandler(ChatHub hubContext)
    {
        _hubContext = hubContext;
    }
    public async Task<AppHandlerResponse> Handle(ChatRequest request,CancellationToken cancellationToken)
    {
        await _hubContext.BroadcastMessage("broadcastMessage", "123");
        return null;
    }
}
