

using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleQuestionAnswer.Commands.DeleteQuestionAnswer;

public record DeleteQuestionAnswerRequest(DeleteQuestionAnswerIn In)
    : IRequest<AppHandlerResponse>;
public class DeleteQuestionAnswerHandler : IRequestHandler<DeleteQuestionAnswerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IUserFilesService _userFilesService;
    public DeleteQuestionAnswerHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response, IUserFilesService userFilesService
        )
    {
        _db = db;
        _response = response;
        _userFilesService = userFilesService;
    }

    public async Task<AppHandlerResponse> Handle(DeleteQuestionAnswerRequest request, CancellationToken cancellationToken)
    {
        var exist = await _db.QuestionAnswers.Include(qa => qa.QuestionAnswerAttachements).Where(c => c.Id == request.In.QuestionAnswerId).SingleOrDefaultAsync(cancellationToken);
        if (exist == null)
        {
            return _response.Error($"Question answer article with id {request.In.QuestionAnswerId} not found or user doesn't have the rights to delete it", AppStatusCodeError.Gone410);
        }
        /** clean up old question answer attachements */
        if (exist.QuestionAnswerAttachements != null)
        {
            while (exist.QuestionAnswerAttachements.Count > 0)
            {
                var oldAttachments = exist.QuestionAnswerAttachements.Last();
                exist.QuestionAnswerAttachements.Remove(oldAttachments);
                _db.QuestionAnswerAttachements.Remove(oldAttachments);
                //try
                //{
                //    await _userFilesService.DeleteBLOBFileAsync(oldAttachments.Attachment!, cancellationToken);
                //}
                //catch (Exception ex)
                //{

                //}
            }
        }
        _db.QuestionAnswers.Remove(exist);
        await _db.SaveChangesAsync(cancellationToken);

        RemoveQuestionAnswerOut removeQuestionAnswerOut = new() { DeletedQuestionAnswerId = request.In.QuestionAnswerId };
        return _response.Success(new DeleteQuestionAnswerOut("Delete article successfully.", removeQuestionAnswerOut));
    }
}