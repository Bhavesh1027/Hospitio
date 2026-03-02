using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Vonage.Models;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared.Enums;
using System.Net.Http.Headers;
using Vonage;
using Vonage.Applications;
using Vonage.Applications.Capabilities;
using Vonage.Common;
using Vonage.Messages;
using Vonage.Messages.Sms;
using Vonage.Messages.WhatsApp;
using Vonage.Numbers;
using Vonage.Request;
using Vonage.SubAccounts.CreateSubAccount;
using VonageComponent = HospitioApi.Core.Services.Vonage.Models.VonageComponent;
using VonageParameter = HospitioApi.Core.Services.Vonage.Models.VonageParameter;

namespace HospitioApi.Core.Services.Vonage;

public class VonageService : IVonageService
{
    private readonly ApplicationDbContext _db;
    private readonly VonageSettingsOptions _vonage;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly FrontEndLinksSettingsOptions _frontEndLinksSettings;
    private readonly ILogger<VonageService> _logger;

    public VonageService(ApplicationDbContext db, IOptions<VonageSettingsOptions> vonage, ITokenGenerator tokenGenerator, IOptions<FrontEndLinksSettingsOptions> frontEndLinksSettings, ILogger<VonageService> logger)
    {
        _db = db;
        _vonage = vonage.Value;
        _tokenGenerator = tokenGenerator;
        _frontEndLinksSettings=frontEndLinksSettings.Value;
        _logger = logger;
    }

    public async Task<dynamic> CreateVonageSubAccount(string subAccountName, int customerId)
    {
        var credentials = Credentials.FromApiKeyAndSecret(_vonage.APIKey, _vonage.APISecret);
        var client = new VonageClient(credentials);
        var request = CreateSubAccountRequest.Build()
            .WithName(subAccountName)
            .WithSecret("Abc@1234")
            .Create();
        var response = await client.SubAccountsClient.CreateSubAccountAsync(request);
        response.Match(
            success =>
            {
                // Access the 'ApiKey' property within the 'success' object
                string apiKey = success.ApiKey;
                VonageCredentials vonageCredentials = new VonageCredentials()
                {
                    CustomerId = customerId,
                    APIKey = apiKey, // Use the 'apiKey' value
                    APISecret = "Abc@1234",
                    SubAccountName = subAccountName
                };
                _db.VonageCredentials.Add(vonageCredentials);
                _db.TestCaseSaveChanges();
                _logger.LogInformation($"SubAccount created - {apiKey}");
            },
            failure =>
            {
                _logger.LogError($"SubAccount creation failed: {failure.GetFailureMessage()} , API Key is {_vonage.APIKey} and API Secret is {_vonage.APISecret} ");

            }
        );

        return response;
    }
    public async Task<dynamic> GetVonageSubAccount()
    {
        var credentials = Credentials.FromApiKeyAndSecret(_vonage.APIKey, _vonage.APISecret);
        var client = new VonageClient(credentials);
        var response = await client.SubAccountsClient.GetSubAccountsAsync();
        var message = response.Match(
            success => $"SubAccounts retrieved - {success.SubAccounts}",
            failure => $"SubAccounts retrieval failed: {failure.GetFailureMessage()}");
        Console.WriteLine(message);

        return response;
    }

    public async Task<dynamic> CreateApplication(string applicationName, int customerId)
    {
        try
        {
            var customerVonageCredentials = await _db.VonageCredentials.Where(e => e.CustomerId == customerId).FirstOrDefaultAsync();
            if (customerVonageCredentials == null)
            {
                _logger.LogError($"Vonage credentials not found for customer ID: {customerId}");
                throw new InvalidOperationException("Vonage credentials not found.");
            }
            else
            {
                var credentials = Credentials.FromApiKeyAndSecret(customerVonageCredentials.APIKey, customerVonageCredentials.APISecret);
                _logger.LogInformation($"Create Vonage Application With API Key : {customerVonageCredentials.APIKey} and API Secret : {customerVonageCredentials.APISecret} ");

                var client = new VonageClient(credentials);

                var messagesWebhooks = new Dictionary<Webhook.Type, Webhook>();
                messagesWebhooks.Add(
                    Webhook.Type.InboundUrl,
                    new Webhook
                    {
                        Address = $"{_vonage.HospitioCallBackBaseURL}api/hospitio-admin/vonage/Get",
                        Method = "POST"
                    });
                messagesWebhooks.Add(
                    Webhook.Type.StatusUrl,
                    new Webhook
                    {
                        Address = $"{_vonage.HospitioCallBackBaseURL}api/hospitio-admin/vonage/Status",
                        Method = "POST"
                    });
                var messagesCapability = new Messages(messagesWebhooks);
                var request = new CreateApplicationRequest
                {
                    Name = applicationName,
                    Capabilities = new ApplicationCapabilities { Messages = messagesCapability }
                };
                var response = await client.ApplicationClient.CreateApplicaitonAsync(request);
                _logger.LogInformation("Vonage Create Application Done .  Response : " + JsonConvert.SerializeObject(response));

                customerVonageCredentials.AppId = response.Id;
                customerVonageCredentials.AppPrivatKey = response.Keys.PrivateKey;
                customerVonageCredentials.AppPublicKey = response.Keys.PublicKey;
                await _db.SaveChangesAsync(CancellationToken.None);

                return response;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex + $" Failed to Create Vonage Application...");
            throw new InvalidOperationException(ex.Message);
        }
    }
    public async Task<dynamic> GetApplications(int pageNo, int pageSize)
    {
        var credentials = Credentials.FromApiKeyAndSecret(_vonage.APIKey, _vonage.APISecret);
        var client = new VonageClient(credentials);

        var request = new ListApplicationsRequest { Page = pageNo, PageSize = pageSize };
        var response = await client.ApplicationClient.ListApplicationsAsync(request);

        Console.WriteLine(JsonConvert.SerializeObject(response));

        return response;
    }
    public async Task<dynamic> SendWhatsappTextMessage(string appId, string privatKey,string senderNumber ,string ReceiverNumber ,string message)
    {
        var credentials = Credentials.FromAppIdAndPrivateKey(appId, privatKey);

        var requestt = new WhatsAppTextRequest
        {
            From =  senderNumber.IndexOf('+') == 0 ? senderNumber.Trim().Substring(1) : senderNumber, //"918320015052",
            To = ReceiverNumber.IndexOf('+') == 0 ? ReceiverNumber.Trim().Substring(1) : ReceiverNumber, //"306980829333",
            Text = message
        };
        //source = (byte)CommunicationPlatFromEnum.Whatsapp;
        var client = new VonageClient(credentials);
        var response = await client.MessagesClient.SendAsync(requestt);

        return response;
    }
    public async Task<dynamic> SendWhatsappAudioMessage(string appId, string privatKey, string message)
    {
        var credentials = Credentials.FromAppIdAndPrivateKey(appId, privatKey);
        var requestt = new WhatsAppAudioRequest
        {
            To = "918320015052",
            From = "306980829333",
            Audio = new Attachment
            {
                Url = message
            }
        };
        //source = (byte)CommunicationPlatFromEnum.Whatsapp;
        var client = new VonageClient(credentials);
        var response = await client.MessagesClient.SendAsync(requestt);

        return response;
    }
    public async Task<dynamic> SendWhatsappFileMessage(string appId, string privatKey, string message)
    {
        var credentials = Credentials.FromAppIdAndPrivateKey(appId, privatKey);
        var requestt = new WhatsAppFileRequest
        {
            To = "918320015052",
            From = "306980829333",
            File = new CaptionedAttachment
            {
                Caption = "Work document",
                Url = message
            }
        };
        //source = (byte)CommunicationPlatFromEnum.Whatsapp;
        var client = new VonageClient(credentials);
        var response = await client.MessagesClient.SendAsync(requestt);

        return response;
    }
    public async Task<dynamic> SendWhatsappImageMessage(string appId, string privatKey, string message)
    {
        var credentials = Credentials.FromAppIdAndPrivateKey(appId, privatKey);
        var requestt = new WhatsAppImageRequest
        {
            To = "918320015052",
            From = "306980829333",
            Image = new CaptionedAttachment
            {
                Caption = "My photo",
                Url = message
            }
        };
        //source = (byte)CommunicationPlatFromEnum.Whatsapp;
        var client = new VonageClient(credentials);
        var response = await client.MessagesClient.SendAsync(requestt);


        return response;
    }
    public async Task<dynamic> SendWhatsappVideoMessage(string appId, string privatKey, string message)
    {
        var credentials = Credentials.FromAppIdAndPrivateKey(appId, privatKey);
        var requestt = new WhatsAppVideoRequest
        {
            To = "918320015052",
            From = "306980829333",
            Video = new CaptionedAttachment
            {
                Caption = "My Video",
                Url = message
            }
        };
        //source = (byte)CommunicationPlatFromEnum.Whatsapp;
        var client = new VonageClient(credentials);
        var response = await client.MessagesClient.SendAsync(requestt);

        return response;
    }

    public async Task<dynamic> SendWhatsappTemplateMessage(string appId, string privatKey, string message, string receiver, string sender, string templateName, List<string> BodyParameters, bool hasButton, Dictionary<int, string> ButtonParameters)
    {
        receiver = receiver.ToString().IndexOf('+') == 0 ? receiver.ToString().Trim().Substring(1) : receiver.ToString();
        sender = sender.ToString().IndexOf('+') == 0 ? sender.ToString().Trim().Substring(1) : sender.ToString();

        var credentials = Credentials.FromAppIdAndPrivateKey(appId, privatKey);

        var client = new VonageClient(credentials);

        #region static template set
        //var templatemessage = new WhatsAppTemplateRequest
        //{
        //    To = "918320015052",
        //    From = "919023728519",
        //    ClientRef = "string",
        //    WhatsApp = new MessageWhatsApp
        //    {
        //        Policy = "deterministic",
        //        Locale = "en-us",
        //    },
        //    Template = new MessageTemplate
        //    {
        //        Name = "ee1abc97_8d63_41a3_88d3_d5d75da14ab0:issue_resolution",
        //        Parameters = new List<object>
        //        {
        //            "luxury inn",
        //            "3",
        //            "september 10th",
        //            "2023"
        //        },
        //    }
        //};
        //var customMessage1 = new WhatsAppCustomRequest
        //{
        //    To = "918320015052",
        //    From = "306980829333",
        //    Custom = new VonageCustom
        //    {
        //        type = "template",
        //        template = new VonageTemplate
        //        {
        //            name = "welcome",
        //            language = new VonageLanguage
        //            {
        //                code = "en",
        //                policy = "deterministic"
        //            },
        //            components = new List<VonageComponent>
        //            {
        //                new()
        //                {
        //                    type= "body",
        //                    parameters = new List<VonageParameter>
        //                    {
        //                        new()
        //                        {
        //                            type= "text",
        //                            text= "Luxury Inn"
        //                        },
        //                        new()
        //                        {
        //                            type= "text",
        //                            text= "3"
        //                        },
        //                        new()
        //                        {
        //                            type= "text",
        //                            text= "September 10th"
        //                        },
        //                        new()
        //                        {
        //                            type= "text",
        //                            text= "September 10th"
        //                        }
        //                    }
        //                },
        //                new()
        //                {
        //                    type= "button",
        //                    index= "0",
        //                    sub_type= "url",
        //                    parameters = new List<VonageParameter>
        //                    {
        //                        new()
        //                        {
        //                            type= "text",
        //                            text= "https://hospitio.appdemoserver.com/dashboard/onboarding"
        //                        }
        //                    }
        //                }

        //            }


        //        }
        //    }
        //};
        #endregion

        #region
        //var customMessage2 = new WhatsAppCustomRequest
        //{
        //    To = "919664639770",
        //    From = "919023728519",
        //    Custom = new VonageCustom
        //    {
        //        type = "template",
        //        template = new VonageTemplate
        //        {
        //            name = "prearrival_27",
        //            language = new VonageLanguage
        //            {
        //                code = "en",
        //                policy = "deterministic"
        //            },
        //            components = new List<VonageComponent>
        //            {
        //                new()
        //                {
        //                    type= "body",
        //                    parameters = new List<VonageParameter>
        //                    {
        //                        new()
        //                        {
        //                            type= "text",
        //                            text= "Pandora"
        //                        },
        //                        new()
        //                        {
        //                            type= "text",
        //                            text= "Vision"
        //                        },
        //                        new()
        //                        {
        //                            type= "text",
        //                            text= "milan"
        //                        },
        //                        new()
        //                        {
        //                            type= "text",
        //                            text= "Vision"
        //                        },
        //                        new()
        //                        {
        //                            type= "text",
        //                            text= "Vision"
        //                        },
        //                        new()
        //                        {
        //                            type= "text",
        //                            text= "Vision"
        //                        },
        //                    }
        //                },
        //                new()
        //                {
        //                    type= "button",
        //                    sub_type= "url",
        //                    index= "0",
        //                    parameters = new List<VonageParameter>
        //                    {
        //                        new()
        //                        {
        //                            type= "text",
        //                            text= "https://www.atJ4PM.org"
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //    }
        //};
        #endregion


        var vonageTemplateComponent = new List<VonageComponent>();
        var vonageBodyParameter = new List<VonageParameter>();
        foreach (var parameter in BodyParameters)
        {
            vonageBodyParameter.Add(new VonageParameter
            {
                type = "text",
                text = parameter
            });
        }
        VonageComponent bodyParameterComponent = new()
        {
            type= "body",
            parameters = vonageBodyParameter
        };
        if (vonageBodyParameter.Count > 0)
        {
            vonageTemplateComponent.Add(bodyParameterComponent);
        }

        if (hasButton && ButtonParameters.Count > 0)
        {
            foreach (var parameter in ButtonParameters)
            {
                var vonageButtonParameter = new List<VonageParameter>();
                vonageButtonParameter.Add(new VonageParameter
                {
                    type = "text",
                    text = parameter.Value
                });
                VonageComponent buttonParameterComponent = new()
                {
                    type= "button",
                    index= parameter.Key.ToString(),
                    sub_type= "url",
                    parameters = vonageButtonParameter
                };
                vonageTemplateComponent.Add(buttonParameterComponent);
            }
        }
        #region
        //var customMessage = new WhatsAppCustomRequest
        //{
        //    To = receiver,
        //    From = sender,
        //    Custom = new VonageCustom
        //    {
        //        type = "template",
        //        template = new VonageTemplate
        //        {
        //            name = templateName,
        //            language = new VonageLanguage
        //            {
        //                code = "en",
        //                policy = "deterministic"
        //            },
        //            components = new List<VonageComponent>
        //    {
        //        new()
        //        {
        //            type= "body",
        //            parameters = vonageBodyParameter
        //        },
        //        new()
        //        {
        //            type= "button",
        //            index= "1",
        //            sub_type= "url",
        //            parameters = vonageButtonParameter
        //        }
        //    }
        //        }
        //    }
        //};
        #endregion

        var customMessage = new WhatsAppCustomRequest
        {
            To = receiver,
            From = sender,
            Custom = new VonageCustom
            {
                type = "template",
                template = new VonageTemplate
                {
                    name = templateName,
                    language = new VonageLanguage
                    {
                        code = "en",
                        policy = "deterministic"
                    },
                    components = vonageTemplateComponent,
                }
            }
        };
        var response = await client.MessagesClient.SendAsync(customMessage);

        return response;
    }

    public async Task<dynamic> SendSMS(string appId, string privatKey,string SenderNumber,string ReceiverNumber,string message)
    {
        var credentials = Credentials.FromAppIdAndPrivateKey(appId, privatKey);

        var vonageClient = new VonageClient(credentials);

        string receiver = ReceiverNumber.ToString().IndexOf('+') == 0 ? ReceiverNumber.ToString().Trim().Substring(1) : ReceiverNumber.ToString();

        string sender = SenderNumber.ToString().IndexOf('+') == 0 ? SenderNumber.ToString().Trim().Substring(1) : SenderNumber.ToString();

        var request = new SmsRequest
        {
            To = receiver,
            From = sender,
            Text = message
        };

        var response = await vonageClient.MessagesClient.SendAsync(request);
        return response;
    }

    public async Task<string> GenerateJWT(string appId, string privatKey)
    {
        var claims = new Dictionary<string, object>
        {
            {
                "acl", new
                {
                    paths = new Dictionary<string, object>
                    {
                        { "/*/rtc/**", new {} },
                        { "/*/users/**", new {} },
                        { "/*/conversations/**", new {} },
                        { "/*/sessions/**", new {} },
                        { "/*/devices/**", new {} },
                        { "/*/image/**", new {} },
                        { "/*/media/**", new {} },
                        { "/*/knocking/**", new {} },
                        { "/*/legs/**", new {} }
                    }
                }
            }
        };

        var response = _tokenGenerator.GenerateToken(appId, privatKey, claims);

        var token = "";
        response.Match(success =>
            {
                token = success;
                Console.WriteLine(success);
            },
            failure =>
            {
                Console.WriteLine(failure);
            });
        return token;
    }

    public async Task<VonageTemplateReponseDto> CreateTemplate(string appId, int Id, string privatKey, string WABAId, dynamic templateData, CancellationToken cancellationToken, string UserType,string stepName ,int CustomerId)
    {
        var token = await GenerateJWT(appId, privatKey);
        Console.WriteLine(token);
        string generateTemplateName = GenerateUniqueTemplateName(Id, stepName);
        string templateName = ModifyTemplateName(generateTemplateName);
        string templateLanguage = "en";
        string templateContent = templateData.TempletMessage;
        string customerPhoneNumber = "";
        string hospitioPhoneNumber = "";
        if (UserType == MessageSenderEnum.Customer.ToString())
        {
            var customers = await _db.Customers.Where(x => x.Id == CustomerId).FirstOrDefaultAsync(cancellationToken);
            customerPhoneNumber = customers.PhoneNumber;
        }
        if (UserType == MessageSenderEnum.Hospitio.ToString())
        {
            var hospitioAdmin = await _db.HospitioOnboardings.FirstOrDefaultAsync(cancellationToken);
            hospitioPhoneNumber = hospitioAdmin.WhatsappNumber;
        }


        // Find placeholders in the template content
        var placeholderMatches = System.Text.RegularExpressions.Regex.Matches(templateContent, @"\{([^}]+)\}");

        // Check if there are placeholders in the template content
        bool hasPlaceholders = placeholderMatches.Count > 0;

        // Replace placeholders with index-based placeholders (e.g., {guest_name} => {{1}}, {hotel_name} => {{2}})
        int placeholderIndex = 1;
        //replacedTemplateContent == "Hello {{1}}, How are you ?{{2}}"
        string replacedTemplateContent = System.Text.RegularExpressions.Regex.Replace(
            templateContent,
            @"\{([^}]+)\}",
            match =>
            {
                string placeholder = match.Groups[1].Value;
                return $"{{{{{placeholderIndex++}}}}}";
            });

        // Create JSON payload
        var templateRequestPayload = new
        {
            name = templateName,
            language = templateLanguage,
            category = "UTILITY",
            components = new List<JObject>
            {
                new JObject
                {
                    { "type", "BODY" },
                    { "text", replacedTemplateContent }
                }
            }
        };

        // Add the example property for the BODY component only if there are placeholders
        if (hasPlaceholders)
        {
            // Generate random strings for each placeholder
            List<string> randomStrings = new List<string>();
            foreach (var match in placeholderMatches)
            {
                randomStrings.Add(GenerateRandomString(10)); // Replace with your random string generation logic
            }

            // Set the "example" property for the BODY component
            (templateRequestPayload.components[0])["example"] = new JObject
            {
                { "body_text", new JArray { new JArray(randomStrings) } }
            };
        }
        //------------Button Code Start --------------------------------
        //Check if the template should include buttons
        if (templateData.Buttons != null && templateData.Buttons.Count > 0)
        {
            var buttons = new List<JObject>();
            foreach (var button in templateData.Buttons)
            {
                var buttonObject = new JObject
                {
                    { "type", button.type },
                    { "text", button.text }
                };
                string tempButtonValue = button.value;
                if (tempButtonValue.Contains("{hotel_phonenumber}"))
                {
                    button.value = (UserType == MessageSenderEnum.Customer.ToString()) ? $"{customerPhoneNumber}" : $"{hospitioPhoneNumber}";
                }
                else if (tempButtonValue.Contains("{guest_url}"))
                {
                    #region For Refrence
                    //button.value = "https://hospitio-guest-dev.appdemoserver.com/{guest_url}";
                    #endregion
                    button.value = _frontEndLinksSettings.GuestPortal + "?id=" + "{guest_url}";
                }

                // Check if the button's value contains placeholders
                var buttonValuePlaceholderMatches = System.Text.RegularExpressions.Regex.Matches(button.value, @"\{([^}]+)\}");

                if (buttonValuePlaceholderMatches.Count > 0)
                {
                    // Generate random strings for placeholders in the button's value
                    List<string> buttonValueRandomStrings = new List<string>();
                    foreach (var match in buttonValuePlaceholderMatches)
                    {
                        buttonValueRandomStrings.Add(GenerateRandomUrl()); // Replace with your random string generation logic
                    }

                    // Set the "example" property for the button
                    buttonObject["example"] = new JArray(buttonValueRandomStrings);
                }

                // Replace placeholders with index-based placeholders (e.g., {guest_name} => {{1}}, {hotel_name} => {{2}})
                int placeholderButtonIndex = 1;
                string buttonValue = button.value;
                string replacedButtonContent = System.Text.RegularExpressions.Regex.Replace(
                    buttonValue,
                    @"\{([^}]+)\}",
                    match =>
                    {
                        string placeholder = match.Groups[1].Value;
                        return $"{{{{{placeholderButtonIndex++}}}}}";
                    });

                if (button.type == "URL")
                {
                    buttonObject["url"] = replacedButtonContent;
                }
                else if (button.type == "PHONE_NUMBER")
                {
                    buttonObject["phone_number"] = replacedButtonContent;
                }

                buttons.Add(buttonObject);
            }

            // Create a JArray to hold the list of buttons
            var buttonsArray = new JArray(buttons);

            templateRequestPayload.components.Add(new JObject
            {
                { "type", "BUTTONS" },
                { "buttons", buttonsArray }
            });
        }


        // Convert template data to JSON
        string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(templateRequestPayload);

        // Vonage Template Management API URL
        string apiUrl = $"https://api.nexmo.com/v2/whatsapp-manager/wabas/{WABAId}/templates";

        using (var httpClient = new HttpClient())
        {
            // Set JWT token in request headers for authorization
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Set content type
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Send POST request to create the template
            var response = await httpClient.PostAsync(apiUrl, new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json"));

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Template created successfully. Response: " + responseBody);
                return new VonageTemplateReponseDto { Status = "success", Response = responseBody, Buttons = templateData.Buttons,  TemplateName = templateName};
            }
            else
            {
                Console.WriteLine("Template creation failed. Status Code: " + response.StatusCode);
                string responseBody = await response.Content.ReadAsStringAsync();
                return new VonageTemplateReponseDto { Status = "failed", Response = responseBody, Buttons = templateData.Buttons,TemplateName = templateName };
            }
        }
    }
    public async Task<VonageTemplateReponseDto> UpdateTemplate(string appId, string privatKey, string WABAId, string templateId, dynamic templateData, CancellationToken cancellationToken, string UserType,string TemplateName ,int CustomerId)
    {
        var token = await GenerateJWT(appId, privatKey);

        string templateName = ModifyTemplateName(TemplateName);
        string templateLanguage = "en";
        string templateContent = templateData.TempletMessage;
        string customerPhoneNumber = "";
        string hospitioPhoneNumber = "";
        if (UserType == MessageSenderEnum.Customer.ToString())
        {
            var customers = await _db.Customers.Where(x => x.Id == CustomerId).FirstOrDefaultAsync(cancellationToken);
            customerPhoneNumber = customers.PhoneNumber;
        }
        if (UserType == MessageSenderEnum.Hospitio.ToString())
        {
            var hospitioAdmin = await _db.HospitioOnboardings.FirstOrDefaultAsync(cancellationToken);
            hospitioPhoneNumber = hospitioAdmin.WhatsappNumber;
        }

        // Find placeholders in the template content
        var placeholderMatches = System.Text.RegularExpressions.Regex.Matches(templateContent, @"\{([^}]+)\}");

        // Check if there are placeholders in the template content
        bool hasPlaceholders = placeholderMatches.Count > 0;

        // Replace placeholders with index-based placeholders (e.g., {guest_name} => {{1}}, {hotel_name} => {{2}})
        int placeholderIndex = 1;
        string replacedTemplateContent = System.Text.RegularExpressions.Regex.Replace(
            templateContent,
            @"\{([^}]+)\}",
            match =>
            {
                string placeholder = match.Groups[1].Value;
                return $"{{{{{placeholderIndex++}}}}}";
            });

        // Create JSON payload
        var templateRequestPayload = new
        {
            name = templateName,
            language = templateLanguage,
            category = "UTILITY",
            components = new List<JObject>
            {
                new JObject
                {
                    { "type", "BODY" },
                    { "text", replacedTemplateContent }
                }
            }
        };

        // Add the example property for the BODY component only if there are placeholders
        if (hasPlaceholders)
        {
            // Generate random strings for each placeholder
            List<string> randomStrings = new List<string>();
            foreach (var match in placeholderMatches)
            {
                randomStrings.Add(GenerateRandomString(10)); // Replace with your random string generation logic
            }

            // Set the "example" property for the BODY component
            (templateRequestPayload.components[0])["example"] = new JObject
            {
                { "body_text", new JArray { new JArray(randomStrings) } }
            };
        }

        //Check if the template should include buttons
        if (templateData.Buttons != null && templateData.Buttons.Count > 0)
        {
            var buttons = new List<JObject>();
            foreach (var button in templateData.Buttons)
            {
                var buttonObject = new JObject
                {
                    { "type", button.type },
                    { "text", button.text }
                };
                string tempButtonValue = button.value;
                if (tempButtonValue.Contains("{hotel_phonenumber}"))
                {
                    button.value = (UserType == MessageSenderEnum.Customer.ToString()) ? $"{customerPhoneNumber}" : $"{hospitioPhoneNumber}";
                }
                else if (tempButtonValue.Contains("{guest_url}"))
                {
                    button.value = _frontEndLinksSettings.GuestPortal + "?id=" + "{guest_url}";
                }
                // Check if the button's value contains placeholders
                var buttonValuePlaceholderMatches = System.Text.RegularExpressions.Regex.Matches(button.value, @"\{([^}]+)\}");

                if (buttonValuePlaceholderMatches.Count > 0)
                {
                    // Generate random strings for placeholders in the button's value
                    List<string> buttonValueRandomStrings = new List<string>();
                    foreach (var match in buttonValuePlaceholderMatches)
                    {
                        buttonValueRandomStrings.Add(GenerateRandomUrl()); // Replace with your random string generation logic
                    }

                    // Set the "example" property for the button
                    buttonObject["example"] = new JArray(buttonValueRandomStrings);
                }

                // Replace placeholders with index-based placeholders (e.g., {guest_name} => {{1}}, {hotel_name} => {{2}})
                int placeholderButtonIndex = 1;
                string buttonValue = button.value;
                string replacedButtonContent = System.Text.RegularExpressions.Regex.Replace(
                    buttonValue,
                    @"\{([^}]+)\}",
                    match =>
                    {
                        string placeholder = match.Groups[1].Value;
                        return $"{{{{{placeholderButtonIndex++}}}}}";
                    });

                if (button.type == "URL")
                {
                    buttonObject["url"] = replacedButtonContent;
                }
                else if (button.type == "PHONE_NUMBER")
                {
                    buttonObject["phone_number"] = replacedButtonContent;
                }

                buttons.Add(buttonObject);
            }

            // Create a JArray to hold the list of buttons
            var buttonsArray = new JArray(buttons);

            templateRequestPayload.components.Add(new JObject
            {
                { "type", "BUTTONS" },
                { "buttons", buttonsArray }
            });
        }


        // Convert template data to JSON
        string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(templateRequestPayload);

        // Vonage Template Management API URL
        string apiUrl = $"https://api.nexmo.com/v2/whatsapp-manager/wabas/{WABAId}/templates/{templateId}";

        using (var httpClient = new HttpClient())
        {
            // Set JWT token in request headers for authorization
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Set content type
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Send POST request to create the template
            var response = await httpClient.PutAsync(apiUrl, new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json"));

            string templateStatus = string.Empty;
            var getTemplateByName = await GETWhatsappTemplateByName(appId, privatKey, WABAId, templateName);
            if (getTemplateByName.Status == "success")
            {
                var root = JsonConvert.DeserializeObject<Root>(getTemplateByName.Response);
                foreach(var item in root.Templates)
                {
                    if (item.Name == templateName)
                    {
                        templateStatus = item.Status;
                    }
                }
            }
            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Template Update successfully. Response: " + responseBody);
                return new VonageTemplateReponseDto { Status = "success", Response = responseBody, Buttons = templateData.Buttons, TemplateName = templateName ,TemplateStatus = templateStatus };
            }
            else
            {
                Console.WriteLine("Template Update failed. Status Code: " + response.StatusCode);
                string responseBody = await response.Content.ReadAsStringAsync();
                return new VonageTemplateReponseDto { Status = "failed", Response = responseBody, Buttons = templateData.Buttons, TemplateName = templateName, TemplateStatus = templateStatus };
            }
        }
    }
    public async Task<dynamic> RemoveWhatsappTemplate(string appId,string privatKey,string WABAId,string TemplateName,string VonageTemplateId)
    {
        var token = await GenerateJWT(appId, privatKey);

        string apiUrl = $"https://api.nexmo.com/v2/whatsapp-manager/wabas/{WABAId}/templates";
        //For Query Parameters
        var uriBuilder = new UriBuilder(apiUrl);
        uriBuilder.Query = $"name={TemplateName}&hsm_id={VonageTemplateId}";

        using (var httpClient = new HttpClient())
        {
            // Set JWT token in request headers for authorization
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Set content type
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Send POST request to create the template
            var response = await httpClient.DeleteAsync(uriBuilder.Uri);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Template Delete successfully. Response: " + responseBody);
                return new { Status = "success", Response = responseBody };
            }
            else
            {
                Console.WriteLine("Template Delete Process failed. Status Code: " + response.StatusCode);
                string responseBody = await response.Content.ReadAsStringAsync();
                return new { Status = "failed", Response = responseBody };
            }
        }
    }
    public async Task<dynamic> GETWhatsappTemplateByName(string appId, string privatKey, string WABAId, string TemplateName)
    {
        var token = await GenerateJWT(appId, privatKey);

        string apiUrl = $"https://api.nexmo.com/v2/whatsapp-manager/wabas/{WABAId}/templates?name={TemplateName}";

        using (var httpClient = new HttpClient())
        {
            // Set JWT token in request headers for authorization
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Set content type
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Send POST request to create the template
            var response = await httpClient.GetAsync(apiUrl);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Template Get successfully. Response: " + responseBody);
                return new { Status = "success", Response = responseBody };
            }
            else
            {
                Console.WriteLine("Template Get Process failed. Status Code: " + response.StatusCode);
                string responseBody = await response.Content.ReadAsStringAsync();
                return new { Status = "failed", Response = responseBody };
            }
        }
    }

    public async Task<dynamic> LinkApplicationToAccount(string appId, string privatKey, string Provider, string ExternalId)
    {
        #region Description For Provider and ExternalId
        //There only Three Provider is Possible : 
        //1.messenger
        //2.viber_service_msg
        //3.whatsapp
        //External id of the account you want to assign an application to. This is channel dependent. For Facebook it will be your Facebook Page ID, for Viber your Viber Service Message ID and for WhatsApp your WhatsApp number.
        #endregion
        var token = await GenerateJWT(appId, privatKey);
        var templateRequestPayload = new
        {
            application = appId
        };
        string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(templateRequestPayload);

        // Vonage Template Management API URL
        string apiUrl = $"https://api.nexmo.com/beta/chatapp-accounts/{Provider}/{ExternalId}/applications";

        using (var httpClient = new HttpClient())
        {
            // Set JWT token in request headers for authorization
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Set content type
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Send POST request to create the template
            var response = await httpClient.PostAsync(apiUrl, new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json"));

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Link Application To Account successfully. Response: " + responseBody);
                return new { Status = "success", Response = responseBody };
            }
            else
            {
                Console.WriteLine("Link Application To Account failed. Status Code: " + response.StatusCode);
                string responseBody = await response.Content.ReadAsStringAsync();
                return new { Status = "failed", Response = responseBody };
            }
        }
    }
    public async Task<dynamic> UnlinkApplicationFromAccount(string appId, string privatKey,string Provider,string ExternalId)
    {
        #region Description For Provider and ExternalId
        //There only Three Provider is Possible : 
        //1.messenger
        //2.viber_service_msg
        //3.whatsapp
        //External id of the account you want to assign an application to. This is channel dependent. For Facebook it will be your Facebook Page ID, for Viber your Viber Service Message ID and for WhatsApp your WhatsApp number.
        #endregion
        var token = await GenerateJWT(appId, privatKey);

        // Vonage Template Management API URL
        string apiUrl = $"https://api.nexmo.com/beta/chatapp-accounts/{Provider}/{ExternalId}/applications/{appId}";

        using (var httpClient = new HttpClient())
        {
            // Set JWT token in request headers for authorization
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Set content type
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Send POST request to create the template
            var response = await httpClient.DeleteAsync(apiUrl);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Unlink Application From Account successfully. Response: " + responseBody);
                return new { Status = "success", Response = responseBody };
            }
            else
            {
                Console.WriteLine("Unlink Application From Account failed. Status Code: " + response.StatusCode);
                string responseBody = await response.Content.ReadAsStringAsync();
                return new { Status = "failed", Response = responseBody };
            }
        }
    }

    public async Task<ResultForRetriveWhatsappAccount> RetriveWhatsappAccount(string appId, string privatKey,string WhatsappNumber)
    {
        var token = await GenerateJWT(appId, privatKey);
        Console.WriteLine(token);
        WhatsappNumber = WhatsappNumber.IndexOf('+') == 0 ? WhatsappNumber.Trim().Substring(1) : WhatsappNumber;
        string apiUrl = $"https://api.nexmo.com/beta/chatapp-accounts/whatsapp/{WhatsappNumber}";

        using (var httpClient = new HttpClient())
        {
            // Set JWT token in request headers for authorization
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Set content type
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Send POST request to create the template
            var response = await httpClient.GetAsync(apiUrl);

            ResultForRetriveWhatsappAccount resultForRetriveWhatsappAccount = new ResultForRetriveWhatsappAccount();
            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Retrive Whatsapp Account successfully. Response: " + responseBody);
                resultForRetriveWhatsappAccount.Status = "success";
                resultForRetriveWhatsappAccount.Response = responseBody;
                return resultForRetriveWhatsappAccount;
            }
            else
            {
                Console.WriteLine("Retrive Whatsapp Account failed. Status Code: " + response.StatusCode);
                string responseBody = await response.Content.ReadAsStringAsync();
                resultForRetriveWhatsappAccount.Status = "failed";
                resultForRetriveWhatsappAccount.Response = responseBody;
                return resultForRetriveWhatsappAccount;
            }
        }

    }
    public async Task<NumberTransactionResponse> BuyNumber(string vonageApiKey, string vonageApiSecret, string countryCode, string vonageNumber)
    {
        var credentials = Credentials.FromApiKeyAndSecret(vonageApiKey, vonageApiSecret);
        var client = new VonageClient(credentials);

        var requests = new NumberTransactionRequest()
        {
            Country = countryCode,
            Msisdn = vonageNumber
        };
        var response = await client.NumbersClient.BuyANumberAsync(requests);
        return response;
    }
    public async Task<NumberTransactionResponse> CancelNumber(string vonageApiKey, string vonageApiSecret, string countryCode, string vonageNumber)
    {
        var credentials = Credentials.FromApiKeyAndSecret(vonageApiKey, vonageApiSecret);
        var client = new VonageClient(credentials);

        var request = new NumberTransactionRequest() { Country = countryCode, Msisdn = vonageNumber };
        var response = await client.NumbersClient.CancelANumberAsync(request);
        return response;
    }
    public async Task<NumbersSearchResponse> ListOwnedNumbers(string vonageApiKey, string vonageApiSecret, string? numberSearchCriteria, SearchPattern? numberSearchPattern)
    {
        var credentials = Credentials.FromApiKeyAndSecret(vonageApiKey, vonageApiSecret);
        var client = new VonageClient(credentials);

        var request = new NumberSearchRequest()
        {
            SearchPattern = numberSearchPattern,
            Pattern = numberSearchCriteria
        };

        var response = await client.NumbersClient.GetOwnedNumbersAsync(request);
        return response;
    }
    public async Task<NumbersSearchResponse> SearchNumbers(string vonageApiKey, string vonageApiSecret, string? country, string? vonageNumberType, string? vonageNumberFeatures, string? numberSearchCriteria, SearchPattern? numberSearchPattern,int? size, int? Index)
    {
        var credentials = Credentials.FromApiKeyAndSecret(vonageApiKey, vonageApiSecret);
        var client = new VonageClient(credentials);

        var request = new NumberSearchRequest()
        {
            Country = country,
            Type = vonageNumberType,
            Features = vonageNumberFeatures,
            Pattern = numberSearchCriteria,
            SearchPattern = numberSearchPattern,
            Size = size,
            Index = Index
        };

        var response = await client.NumbersClient.GetAvailableNumbersAsync(request);
        return response;
    }
    public async Task<NumberTransactionResponse> UpdateNumber(string vonageApiKey, string vonageApiSecret, string countryCode, string vonageNumber, string vonageApplicationId)
    {
        var credentials = Credentials.FromApiKeyAndSecret(vonageApiKey, vonageApiSecret);
        var client = new VonageClient(credentials);

        var request = new UpdateNumberRequest()
        {
            Country = countryCode,
            Msisdn = vonageNumber,
            AppId = vonageApplicationId,
            //MoHttpUrl = smsCallbackUrl,
            //VoiceCallbackType = voiceCallbackType,
            //VoiceCallbackValue = voiceCallbackValue,
            //VoiceStatusCallback = voiceStatusUrl
        };

        var response = await client.NumbersClient.UpdateANumberAsync(request);
        return response;
    }
    static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new Random();
        char[] randomChars = new char[length];

        for (int i = 0; i < length; i++)
        {
            randomChars[i] = chars[random.Next(chars.Length)];
        }

        return new string(randomChars);
    }
    static string GenerateRandomUrl()
    {
        string baseurl = "https://www.";
        Random random = new Random();
        string subdomain = GenerateRandomString(6);
        string[] top_level_domains = { "com", "net", "org" };
        string tld = top_level_domains[random.Next(top_level_domains.Length)];

        return $"{baseurl}{subdomain}.{tld}";
    }
    static string ModifyTemplateName(string templateName)
    {
        templateName = templateName.TrimStart().TrimEnd().Trim();
        string newTemplateName = string.Empty;
        var a = templateName.Split();
        for (int i = 0; i<a.Length; i++)
        {
            if (a[i] != "")
            {
                if (i != a.Length-1)
                {
                    newTemplateName += a[i] + '_';
                }
                else
                {
                    newTemplateName += a[i];
                }
            }
        }
        return newTemplateName.ToLower();
    }
    static string GenerateUniqueTemplateName(int Id,string StepName)
    {
        return $"{StepName}_{Id}";
    }
}
public class ResultForRetriveWhatsappAccount
{
    public string Status { get; set; }
    public string Response { get; set; }
}
public class Cursors
{
    public string Before { get; set; }
    public string After { get; set; }
}

public class Paging
{
    public Cursors Cursors { get; set; }
}

public class Example
{
    public List<List<string>> BodyText { get; set; }
}

public class Component
{
    public string Type { get; set; }
    public string Text { get; set; }
    public Example Example { get; set; }
}

public class Button
{
    public string Type { get; set; }
    public string Text { get; set; }
    public string Url { get; set; }
    public List<string> Example { get; set; }
    public string PhoneNumber { get; set; }
}

public class Template
{
    public string Name { get; set; }
    public List<Component> Components { get; set; }
    public string Language { get; set; }
    public string Status { get; set; }
    public string Category { get; set; }
    public string Id { get; set; }
}

public class Root
{
    public Paging Paging { get; set; }
    public List<Template> Templates { get; set; }
}
