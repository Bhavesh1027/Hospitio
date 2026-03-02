using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleAccount.Commands.ResetPasswordConfirmation;

public record ResetPasswordConfirmationHandlerRequest(ResetPasswordConfirmationIn In)
    : IRequest<AppHandlerResponse>;

public class ResetPasswordConfirmationHandler : IRequestHandler<ResetPasswordConfirmationHandlerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ISendEmail _mail;
    private readonly SMTPEmailSettingsOptions _smtpEmailSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJwtService _jwtService;
    private readonly ILogger<ResetPasswordConfirmationHandler> _logger;
    private readonly IHostingEnvironment Environment;
    private readonly ResetPasswordSettingsOptions _resetPasswordSettings;

    public ResetPasswordConfirmationHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response,
        ISendEmail sendEmail,
        IOptions<SMTPEmailSettingsOptions> smtpEmailSettings,
        IHttpContextAccessor httpContextAccessor,
        IJwtService jwtService,
        ILogger<ResetPasswordConfirmationHandler> logger,
        IHostingEnvironment _environment,
        IOptions<ResetPasswordSettingsOptions> resetPasswordSettings)
    {
        _db = db;
        _response = response;
        _mail = sendEmail;
        _smtpEmailSettings = smtpEmailSettings.Value;
        _httpContextAccessor = httpContextAccessor;
        _jwtService = jwtService;
        _logger = logger;
        Environment = _environment;
        _resetPasswordSettings = resetPasswordSettings.Value;
    }

    public async Task<AppHandlerResponse> Handle(ResetPasswordConfirmationHandlerRequest request, CancellationToken cancellationToken)
    {
        string email = string.Empty;
        string token = string.Empty;

        if (request.In.IsUser)
        {
            var user = await _db.Users.Include(i => i.UserLevel).FirstOrDefaultAsync(x => x.Email == request.In.Email, cancellationToken);

            if (user == null)
            {
                return _response.Error("User not found.", AppStatusCodeError.Forbidden403);
            }

            email = user.Email;
            token = _jwtService.GenerateJWTResetPasswordTokenAsync(user);
        }
        else if (!request.In.IsUser)
        {
            var customer = await _db.CustomerUsers.FirstOrDefaultAsync(x => x.Email == request.In.Email, cancellationToken);

            if (customer == null)
            {
                return _response.Error("Customer not found.", AppStatusCodeError.Forbidden403);
            }

            email = customer.Email;
            token = _jwtService.GenerateJWTResetPasswordTokenForCustomerAsync(customer);
        }

        var portalBaseUri = new Uri(_httpContextAccessor.HttpContext!.Request.Headers.Referer);

        string resetPasswordUrl = $@"{portalBaseUri}reset-password/{token}";

        var fullEmailBody = PopulateEmailTemplate(resetPasswordUrl, email, _smtpEmailSettings.FromEmail);

        SendEmailOptions sendEmail = new SendEmailOptions();
        sendEmail.Subject = _resetPasswordSettings.Subject;
        sendEmail.Addresslist = email;
        sendEmail.IsHTML = true;
        sendEmail.Body = fullEmailBody;
        sendEmail.IsNoReply = _resetPasswordSettings.IsNoReply;

        await _mail.ExecuteAsync(sendEmail, cancellationToken);

        return _response.Success(new ResetPasswordConfirmationOut("Reset password email sent successful."));
    }

    private string PopulateEmailTemplate(string link, string toEmail, string fromEmail)
    {

        string emailTemplate = string.Empty;
        string path = Path.Combine(this.Environment.WebRootPath, "html/") + _resetPasswordSettings.EmailTemplate;

        try
        {
            using (var reader = new StreamReader(path))
            {
                emailTemplate = reader.ReadToEnd();
            }

            if (!string.IsNullOrWhiteSpace(link))
            {
                /* We Insert Reset link Dynamic (passing by caller method) */
                string resetLink = $@"<a href=""{link}"" target=""_blank"" style=""
      text-decoration: none;
      display: inline-block;
      color: #ffffff;
      background-color: #5400cf;
      border-radius: 10px;
      width: auto;
      border-top: 0px solid transparent;
      font-weight: undefined;
      border-right: 0px solid transparent;
      border-bottom: 0px solid transparent;
      border-left: 0px solid transparent;
      padding-top: 10px;
      padding-bottom: 10px;
      font-family: Arial, Helvetica Neue, Helvetica, sans-serif;
      font-size: 16px;
      text-align: center;
      mso-border-alt: none;
      word-break: keep-all;
    "">
                            <span style=""
        padding-left: 40px;
        padding-right: 40px;
        font-size: 16px;
        display: inline-block;
        letter-spacing: normal;
      "">
                                <span style=""word-break: break-word; line-height: 32px"">Reset Your Password</span>
                            </span>
                        </a>";

                string toEmailBody = $@"<a href=""{toEmail}"">{toEmail}</a>";

                string fromEmailBody = $@"<a href=""{fromEmail}"">{fromEmail}</a>.";

                emailTemplate = emailTemplate.Replace("<!--{resetlink}-->", resetLink);
                emailTemplate = emailTemplate.Replace("<!--{toEmail}-->", toEmailBody);
                emailTemplate = emailTemplate.Replace("<!--{fromEmail}-->", fromEmailBody);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while reading email template in reset password.");
        }
        return emailTemplate;
    }
}
