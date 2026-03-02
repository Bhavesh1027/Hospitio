using MediatR;
using HospitioApi.Data;
using HospitioApi.Shared;


namespace HospitioApi.Pipeline;

public class DbTransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : AppHandlerResponse
{
    private readonly ApplicationDbContext _db;

    public DbTransactionPipelineBehavior(
        ApplicationDbContext db
        )
    {
        _db = db;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
        )
    {
        using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        var response = await next();
        if (response.HasFailureAndRollbackOnFailure)
        {
            await transaction.RollbackAsync(cancellationToken);
        }
        else /** If success (or failure with partial changes needed to be saved/commited) */
        {
            await transaction.CommitAsync(cancellationToken);
        }
        return response;
    }
}
