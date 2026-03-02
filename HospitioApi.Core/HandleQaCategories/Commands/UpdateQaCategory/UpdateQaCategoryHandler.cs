using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleQaCategories.Commands.UpdateQaCategory;
public record UpdateQaCategoryRequest(UpdateQaCategoryIn In) : IRequest<AppHandlerResponse>;
public class UpdateQaCategoryHandler : IRequestHandler<UpdateQaCategoryRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdateQaCategoryHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateQaCategoryRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.QuestionAnswerCategories.Where(e => e.Name == request.In.Name && e.Id != request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The QA category {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
        }
        var qaCategory = await _db.QuestionAnswerCategories.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (qaCategory == null)
        {
            return _response.Error($"QA category with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        qaCategory.Name = request.In.Name;

        await _db.SaveChangesAsync(cancellationToken);

        var updateQaCategoryOut = new UpdatedQaCategoryOut()
        {
            Id = qaCategory.Id,
            Name = qaCategory.Name,
        };

        return _response.Success(new UpdateQaCategoryOut("Update qa Category successful.", updateQaCategoryOut));
    }
}
