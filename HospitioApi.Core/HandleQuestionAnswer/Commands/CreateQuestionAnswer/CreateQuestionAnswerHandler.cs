using Dapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleQuestionAnswer.Commands.CreateQuestionAnswer;

public record CreateQuestionAnswerRequest(CreateQuestionAnswerIn In) : IRequest<AppHandlerResponse>;

public class CreateQuestionAnswerHandler : IRequestHandler<CreateQuestionAnswerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IHandlerResponseFactory _response;
    private readonly IChatService _chatService;

    public CreateQuestionAnswerHandler(ApplicationDbContext db, IHubContext<ChatHub> hubContext, IChatService chatService,IHandlerResponseFactory response)
    {
        _db = db;
        _hubContext = hubContext;
        _chatService = chatService;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateQuestionAnswerRequest request, CancellationToken cancellationToken)
    {

        var checkExist = await _db.QuestionAnswerCategories.Where(e => e.Id == request.In.QuestionAnswerCategoryId).SingleOrDefaultAsync(cancellationToken);
        if (checkExist == null)
        {
            return _response.Error($"Given question answers category not exists.", AppStatusCodeError.UnprocessableEntity422);
        }

        var quesAnsIn = request.In;
        var questionAnswer = new QuestionAnswer
        {
            QuestionAnswerCategoryId = quesAnsIn.QuestionAnswerCategoryId,
            Name = quesAnsIn.Name,
            Description = quesAnsIn.Description,
            Icon = quesAnsIn.Icon,
            IsActive = quesAnsIn.IsActive,
            IsPublish = quesAnsIn.IsPublish,
        };
        await _db.QuestionAnswers.AddAsync(questionAnswer, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        string Attachment = string.Empty;
        string AttachmentType = string.Empty;
        if (quesAnsIn.QuestionAnswerAttachements.Any())
        {
            List<QuestionAnswerAttachement> listOfAttachments = new();
            foreach (var attachment in quesAnsIn.QuestionAnswerAttachements)
            {
                var QuestionAnswerAttachement = new QuestionAnswerAttachement
                {
                    QuestionAnswerId = questionAnswer.Id,
                    Attachment = attachment.Attachment,
                    AttachmentType = attachment.AttachmentType,
                };
                listOfAttachments.Add(QuestionAnswerAttachement);
                Attachment = attachment.Attachment;
                AttachmentType = attachment.AttachmentType;
            }
            await _db.QuestionAnswerAttachements.AddRangeAsync(listOfAttachments, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }
        var createdQuestionAnswerOut = new CreatedQuestionAnswerOut
        {
            Id = questionAnswer.Id
        };

        var customerUsers = await _db.CustomerUsers.Where(e=>e.DeletedAt == null).ToListAsync(cancellationToken);

        foreach (var user in customerUsers)
        {
            var dymanicQuestionAnswer = new
            {
                Attachment = Attachment,
                AttachmentType = AttachmentType,
                Description = questionAnswer.Description,
                Icon = questionAnswer.Icon,
                Id = createdQuestionAnswerOut.Id,
                IsActive = questionAnswer.IsActive,
                Name = questionAnswer.Name,
                Status = "New"
            };
            await _hubContext.Clients.Group($"user-2-{user.Id}").SendAsync("GetNewQuestionAnswer", dymanicQuestionAnswer);
           
            var totalUnreadQACount = await _chatService.GetTotalUnreadQACount(user.Id.ToString());
            var totalUnreadQACountResponse = new { Type = "QA", Count = totalUnreadQACount };
            await _hubContext.Clients.Group($"user-2-{user.Id}").SendAsync("GetTotalUnreadCount", totalUnreadQACountResponse);
        }

        return _response.Success(new CreateQuestionAnswerOut("Create question answer successful.", createdQuestionAnswerOut));
    }
}