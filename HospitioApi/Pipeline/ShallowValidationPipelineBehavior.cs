using FluentValidation;
using FluentValidation.Results;
using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;


namespace HospitioApi.Pipeline;

public class ShallowValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : AppHandlerResponse
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IHandlerResponseFactory _response;

    public ShallowValidationPipelineBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        IHandlerResponseFactory response
        )
    {
        _validators = validators;
        _response = response;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
        )
    {
        List<ValidationFailure> validationFailures = _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(results => results.Errors)
            .Where(error => error != null)
            .ToList();

        if (validationFailures.Any())
        {
            return (TResponse)_response.Error(new AppValidationException(validationFailures));
        }

        return await next();
    }
}
