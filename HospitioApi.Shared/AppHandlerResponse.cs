

namespace HospitioApi.Shared;
public class AppHandlerResponse
{
    private bool? _rollbackOnFailure;

    public AppHandlerResponse(BaseResponseOut response)
    {
        Response = response;
        Failure = null;
        _rollbackOnFailure = null;
    }

    public AppHandlerResponse(AppException ex, bool rollbackOnFailure = true)
    {
        Response = null;
        Failure = ex;
        _rollbackOnFailure = rollbackOnFailure;
    }

    public bool HasResponse => Response is not null;
    public bool HasFailure => Failure is not null;
    public bool HasFailureAndRollbackOnFailure => Failure is not null && _rollbackOnFailure.GetValueOrDefault(true);

    public BaseResponseOut? Response { get; private set; }
    public AppException? Failure { get; private set; }

    public void SetResponse(BaseResponseOut response)
    {
        Response = response;
        Failure = null;
        _rollbackOnFailure = null;
    }

    public void SetFailure(AppException ex, bool rollbackOnFailure = true)
    {
        Response = null;
        Failure = ex;
        _rollbackOnFailure = rollbackOnFailure;
    }
}

