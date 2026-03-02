
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Shared;
//using SendGrid;
//using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System.Net;
//using System.Text;
//using System.Net.Mime;

namespace HospitioApi.Core.Services.SendEmail;

public class SendEmailOptions
{
	public string? Subject { get; set; }
	public string? FromEmail { get; set; }
	public string? FromName { get; set; }
	public string Addresslist { get; set; } = string.Empty;
	public string? ToName { get; set; }
	public string? BCC { get; set; }
	public string? CC { get; set; }
	public bool IsHTML { get; set; }
	public string? Body { get; set; }
	public bool IsNoReply { get; set; }
	public string? MaskEmail { get; set; }
	public List<Attachment>? Attachments { get; set; }
	public Attachment? Attachment { get; set; }
}

public class SendEmail : ISendEmail
{
	private readonly ILogger<SendEmail> _logger;
	//private readonly SendGridClient _client;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly EmailNotificationSettingsOptions _emailNotificationSettings;
	private readonly SMTPEmailSettingsOptions _smtpEmailSettings;

	public SendEmail(
		ILogger<SendEmail> logger,
		IHttpContextAccessor httpContextAccessor,
		IOptions<EmailNotificationSettingsOptions> emailNotificationSettings,
		IOptions<SMTPEmailSettingsOptions> smtpEmailSettings)
	{
		//_client = new SendGridClient(emailNotificationSettings.Value.SendGridAPIKey);
		_logger = logger;
		_httpContextAccessor = httpContextAccessor;
		_emailNotificationSettings = emailNotificationSettings.Value;
		_smtpEmailSettings = smtpEmailSettings.Value;
	}

	public async Task<bool> ExecuteAsync(SendEmailOptions options, CancellationToken cancellationToken)
	{
		try
		{
			//Using SMTP
			var smtpClient = new SmtpClient(_smtpEmailSettings.Server)
			{
				Port = _smtpEmailSettings.Port,
				Credentials = new NetworkCredential(_smtpEmailSettings.UserName, _smtpEmailSettings.Password),
				EnableSsl = _smtpEmailSettings.EnableSsl,
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress(!string.IsNullOrEmpty(options.MaskEmail) ? options.MaskEmail : _smtpEmailSettings.FromEmail),
				Subject = options.Subject,
				Body = PrepareBody(options),
				IsBodyHtml = options.IsHTML,
			};

			string[] addresses = options.Addresslist.Split(";, \r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

			foreach (var to in addresses)
			{
				mailMessage.To.Add(to);
			}

			if (options.Attachments != null)
			{
				//var attachment = new Attachment(options.Attachments[0]); //@"D:\logo.png"
				foreach (var attachment in options.Attachments)
				{
					mailMessage.Attachments.Add(attachment);
				}
			}
			if (options.Attachment != null)
			{
				mailMessage.Attachments.Add(options.Attachment);
			}

			smtpClient.Send(mailMessage);

			//Using SendGrid

			//var emailNoReply = _emailNotificationSettings.EmailNoReply;
			//var from = new EmailAddress(options.FromEmail ?? emailNoReply, options.FromName ?? "Hospitio");
			//var subject = options.Subject;
			//var htmlContent = PrepareBody(options);
			//string[] addresses = options.Addresslist.Split(";, \r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			//var to = from;
			//if (addresses.Length > 0)
			//{
			//    to = new EmailAddress(addresses[0], options.ToName);
			//}

			//var objMM = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, htmlContent);

			//var emails = new List<EmailAddress>();

			//for (int i = 1; i < addresses.Length; i++)
			//{
			//    emails.Add(new EmailAddress(addresses[i]));
			//}
			//if (emails.Count > 0)
			//{
			//    objMM.AddTos(emails);
			//}

			//if (options.Attachments is not null)
			//{
			//    foreach (string att in options.Attachments)
			//    {
			//        byte[] byteData = Encoding.ASCII.GetBytes(File.ReadAllText(att));
			//        objMM.AddAttachment(new Attachment
			//        {
			//            Content = Convert.ToBase64String(byteData),
			//            Filename = "Transcript.txt",
			//            Type = "txt/plain",
			//            Disposition = "attachment"
			//        });
			//    }
			//}

			//addresses = (options.CC ?? "").Split(";, \r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			//emails.Clear();
			//foreach (string address in addresses)
			//{
			//    emails.Add(new EmailAddress(address));
			//}
			//if (emails.Count > 0)
			//{
			//    objMM.AddCcs(emails);
			//}

			//addresses = (options.BCC ?? "").Split(";, \r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			//emails.Clear();
			//foreach (string address in addresses)
			//{
			//    emails.Add(new EmailAddress(address));
			//};
			//if (emails.Count > 0)
			//{
			//    objMM.AddBccs(emails);
			//}

			///* for single Attachment */
			//if (options.Attachment != null)
			//{
			//    objMM.AddAttachment(options.Attachment);
			//}

			//var response = await _client.SendEmailAsync(objMM, cancellationToken);
			//if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
			//{
			//    _logger.LogError($"from: {objMM.From.Email} fromName: {objMM.From.Name} CC : {options.CC} BCC: {options.BCC} To: {objMM.Personalizations[0].Tos[0].Email} ToName: {objMM.Personalizations[0].Tos[0].Name} response.StatusCode : {response.StatusCode}");
			//    return false;
			//}
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Error during send email.");
			return false;
		}
		return true;
	}

	private string PrepareBody(SendEmailOptions options)
	{
		const string noReplyMessage = "Please do not reply to this email as this email address is not monitored.";

		if (options.IsNoReply)
		{
			return options.IsHTML
				? PrepareBodyHtml($"{options.Body}<br />{noReplyMessage}")
				: $"{options.Body} {noReplyMessage}";
		}

		return options.IsHTML ? PrepareBodyHtml(options.Body ?? "") : (options.Body ?? "");
	}

	private string PrepareBodyHtml(string html)
	{
		var doc = new HtmlDocument();
		doc.LoadHtml(html);
		var links = doc.DocumentNode.Descendants("a");
		foreach (var link in links)
		{
			string href = link.GetAttributeValue("href", "");
			if (!string.IsNullOrEmpty(href))
			{
				if (!href.StartsWith("http"))
				{
					var isSecureConnection = _httpContextAccessor.HttpContext?.Request.IsHttps ?? false;
					if (_httpContextAccessor.HttpContext is not null)
					{
						_logger.LogInformation("Original href: {href}", href);
						href = href.ToAbsoluteUrl(_httpContextAccessor.HttpContext, isSecureConnection);
					}
					_logger.LogInformation("Absolute href built: {href}", href);
					link.Attributes["href"].Value = href;
				}
			}
		}
		return doc.DocumentNode.OuterHtml;
	}
}