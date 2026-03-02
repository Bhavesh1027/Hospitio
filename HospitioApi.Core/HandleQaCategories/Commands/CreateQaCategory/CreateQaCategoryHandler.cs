using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;


namespace HospitioApi.Core.HandleQaCategories.Commands.CreateQaCategory;

public record CreateQaCategoryRequest(CreateQaCategoryIn In) : IRequest<AppHandlerResponse>;

public class CreateQaCategoryHandler : IRequestHandler<CreateQaCategoryRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateQaCategoryHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateQaCategoryRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.QuestionAnswerCategories.Where(e => e.Name == request.In.Name).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The qa category {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
        }
        var qacategory = new QuestionAnswerCategory
        {
            Name = request.In.Name,
            IsActive = true
        };
        await _db.QuestionAnswerCategories.AddAsync(qacategory, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        var createQaCategoryOut = new CreatedQaCategoryOut
        {
            Id = qacategory.Id,
            Name = qacategory.Name
        };
        return _response.Success(new CreateQaCategoryOut("Create question category successful.", createQaCategoryOut));
    }
}
