

using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleQuestionAnswer.Commands.UpdateQuestionAnswer;

public record UpdateQuestionAnswerRequest(UpdateQuestionAnswerIn In)
    : IRequest<AppHandlerResponse>;

public class UpdateQuestionAnswerHandler : IRequestHandler<UpdateQuestionAnswerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IHandlerResponseFactory _response;

    public UpdateQuestionAnswerHandler(
        ApplicationDbContext db,
        IHubContext<ChatHub> hubContext,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _hubContext = hubContext;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateQuestionAnswerRequest request, CancellationToken cancellationToken)
    {
        var QAReq = await _db.QuestionAnswers.Where(r => r.Id == request.In.Id).SingleOrDefaultAsync(cancellationToken);
        if (QAReq == null)
        {
            return _response.Error("QA request not found.", AppStatusCodeError.Gone410);
        }
        QAReq.IsActive = request.In.IsActive;
        await _db.SaveChangesAsync(cancellationToken);

        string Attachment = string.Empty;
        string AttachmentType = string.Empty;

        var QAattachment = await _db.QuestionAnswerAttachements.Where(c => c.QuestionAnswerId == QAReq.Id).SingleOrDefaultAsync(cancellationToken);
        if (QAattachment != null)
        {
            Attachment = QAattachment.Attachment;
            AttachmentType = QAattachment.AttachmentType;
        }
        var QAReqs = new
        {
            Attachment = Attachment,
            AttachmentType = AttachmentType,
            Description = QAReq.Description,
            Icon = QAReq.Icon,
            Id = QAReq.Id,
            IsActive = QAReq.IsActive,
            Name = QAReq.Name,
            Status = "Update"
        };

        var customerUsers = await _db.CustomerUsers.Where(e => e.DeletedAt == null).ToListAsync(cancellationToken);

        foreach (var user in customerUsers)
        {
            await _hubContext.Clients.Group($"user-2-{user.Id}").SendAsync("GetNewQuestionAnswer", QAReqs);
        }

        return _response.Success(new UpdateQuestionAnswerStatusOut("Update request status successfully."));
    }
}