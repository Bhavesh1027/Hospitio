using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using Microsoft.AspNetCore.SignalR;
using HospitioApi.Core.SignalR.Hubs;

namespace HospitioApi.Core.HandleQuestionAnswer.Commands.EditQuestionAnswer;

public record EditQuestionAnswerRequest(EditQuestionAnswerIn In) : IRequest<AppHandlerResponse>;
public class EditQuestionAnswerHandler : IRequestHandler<EditQuestionAnswerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IUserFilesService _userFilesService;
    private readonly IHubContext<ChatHub> _hubContext;

    public EditQuestionAnswerHandler(ApplicationDbContext db, IHandlerResponseFactory response, IUserFilesService userFilesService, IHubContext<ChatHub> hubContext)
    {
        _db = db;
        _response = response;
        _userFilesService = userFilesService;
        _hubContext = hubContext;
    }
    public async Task<AppHandlerResponse> Handle(EditQuestionAnswerRequest request, CancellationToken cancellationToken)
    {
        var quesAnsIn = request.In;
        var checkExist = await _db.QuestionAnswers.Include(qa => qa.QuestionAnswerAttachements).Where(e => e.Id == quesAnsIn.Id).SingleOrDefaultAsync(cancellationToken);
        if (checkExist == null)
        {
            return _response.Error($"Given article not exists.", AppStatusCodeError.UnprocessableEntity422);
        }

        checkExist.QuestionAnswerCategoryId = quesAnsIn.QuestionAnswerCategoryId;
        checkExist.Name = quesAnsIn.Name;
        checkExist.Description = quesAnsIn.Description;
        checkExist.Icon = quesAnsIn.Icon;
        checkExist.IsActive = quesAnsIn.IsActive;
        checkExist.IsPublish = quesAnsIn.IsPublish;

        string Attachment = string.Empty;
        string AttachmentType = string.Empty;
        if (quesAnsIn.QuestionAnswerAttachements.Any())
        {
            /** clean up old question answer attachements */
            if (checkExist.QuestionAnswerAttachements != null)
            {
                while (checkExist.QuestionAnswerAttachements.Count > 0)
                {
                    var oldAttachments = checkExist.QuestionAnswerAttachements.Last();
                    checkExist.QuestionAnswerAttachements.Remove(oldAttachments);
                    _db.QuestionAnswerAttachements.Remove(oldAttachments);
                    //try
                    //{
                    // await  _userFilesService.DeleteBLOBFileAsync(oldAttachments.Attachment!, cancellationToken);
                    //}
                    //catch(Exception ex)
                    //{

                    //}
                }
            }
            List<QuestionAnswerAttachement> listOfAttachments = new();
            foreach (var attachment in quesAnsIn.QuestionAnswerAttachements)
            {
                var QuestionAnswerAttachement = new QuestionAnswerAttachement
                {
                    QuestionAnswerId = checkExist.Id,
                    Attachment = attachment.Attachment,
                    AttachmentType = attachment.AttachmentType,
                };
                listOfAttachments.Add(QuestionAnswerAttachement);
                Attachment = attachment.Attachment;
                AttachmentType = attachment.AttachmentType;
            }
            await _db.QuestionAnswerAttachements.AddRangeAsync(listOfAttachments, cancellationToken);

        }
        await _db.SaveChangesAsync(cancellationToken);
        var editedQuestionAnswerOut = new EditedQuestionAnswerOut
        {
            Id = checkExist.Id
        };

        var customerUsers = await _db.CustomerUsers.Where(e => e.DeletedAt == null).ToListAsync(cancellationToken);
        foreach (var user in customerUsers)
        {
            var dymanicQuestionAnswer = new
            {
                Attachment = Attachment,
                AttachmentType = AttachmentType,
                Description = checkExist.Description,
                Icon = checkExist.Icon,
                Id = editedQuestionAnswerOut.Id,
                IsActive = checkExist.IsActive,
                Name = checkExist.Name,
                Status = "Update"
            };
            await _hubContext.Clients.Group($"user-2-{user.Id}").SendAsync("GetNewQuestionAnswer", dymanicQuestionAnswer);
        }

        return _response.Success(new EditQuestionAnswerOut("Article edited successfully.", editedQuestionAnswerOut));
    }
}