

using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleQuestionAnswer.Commands.UpdateQuestionAnswerStatus;

public record UpdateQuestionAnswerStatusRequest(UpdateQuestionAnswerStatusIn In)
    : IRequest<AppHandlerResponse>;

public class UpdateQuestionAnswerStatusHandler : IRequestHandler<UpdateQuestionAnswerStatusRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public UpdateQuestionAnswerStatusHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateQuestionAnswerStatusRequest request, CancellationToken cancellationToken)
    {
        var QAReq = await _db.QuestionAnswers.Where(r => r.Id == request.In.Id).SingleOrDefaultAsync(cancellationToken);
        if (QAReq == null)
        {
            return _response.Error("QA request not found.", AppStatusCodeError.Gone410);
        }
        if (QAReq.IsActive == false && request.In.IsPublish)
        {
            return _response.Error("Request is inactive unable to publish this request.", AppStatusCodeError.Gone410);
        }
        QAReq.IsPublish = request.In.IsPublish;
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdateQuestionAnswerStatusOut("Update request status successfully."));
    }
}