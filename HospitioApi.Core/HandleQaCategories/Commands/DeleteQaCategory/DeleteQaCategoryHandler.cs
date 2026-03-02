using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Linq.Dynamic.Core;

namespace HospitioApi.Core.HandleQaCategories.Commands.DeleteQaCategory;
public record DeleteQaCategoryRequest(DeleteQaCategoryIn In) : IRequest<AppHandlerResponse>;
public class DeleteQaCategoryHandler : IRequestHandler<DeleteQaCategoryRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public DeleteQaCategoryHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(DeleteQaCategoryRequest request, CancellationToken cancellationToken)
    {
        var qaCategory = await _db.QuestionAnswerCategories.Where(e => e.Id == request.In.QaCategoryId).FirstOrDefaultAsync(cancellationToken);
        if (qaCategory == null)
        {
            return _response.Error($"Qa category with Id {request.In.QaCategoryId} could not be found.", AppStatusCodeError.Gone410);
        }
        _db.QuestionAnswerCategories.Remove(qaCategory);
        await _db.SaveChangesAsync(cancellationToken);


        var qaAnswer = await _db.QuestionAnswers.Where(e => e.QuestionAnswerCategoryId == qaCategory.Id).ToListAsync();

        if (qaAnswer != null && qaAnswer.Count > 0)
        {
            foreach (var item in qaAnswer)
            {
                var qaAttechment = await _db.QuestionAnswerAttachements.Where(e => e.QuestionAnswerId == item.Id).ToListAsync();

                _db.QuestionAnswerAttachements.RemoveRange(qaAttechment);
                await _db.SaveChangesAsync(cancellationToken);

            }

            _db.QuestionAnswers.RemoveRange(qaAnswer);
            await _db.SaveChangesAsync(cancellationToken);

        }

        return _response.Success(new DeleteQaCategoryOut("Delete qa category successful.", new() { QaCategoryId = qaCategory.Id }));
    }
}

