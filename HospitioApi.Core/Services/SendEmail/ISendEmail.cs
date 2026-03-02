

namespace HospitioApi.Core.Services.SendEmail;

public interface ISendEmail
{
    Task<bool> ExecuteAsync(SendEmailOptions options, CancellationToken cancellationToken);
}