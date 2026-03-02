using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml.Drawing.Slicer.Style;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared;
using System.Text;
using Vonage;
using Vonage.Common;
using Vonage.Messages.WhatsApp;
using Vonage.Request;

namespace HospitioApi.Core.HandleVonage.Commands.Vonage;

public record VonageRequest() : IRequest<AppHandlerResponse>;

public class VonageHandler : IRequestHandler<VonageRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly ISendEmail _sendEmail;
    private readonly IVonageService _vonageService;
    private readonly ApplicationDbContext _db;

    public VonageHandler(
        IHandlerResponseFactory response,
        ISendEmail sendEmail,
        IVonageService vonageService,
        ApplicationDbContext db
        )
    {
        _response = response;
        _sendEmail = sendEmail;
        _vonageService = vonageService;
        _db = db;
    }

    public async Task<AppHandlerResponse> Handle(VonageRequest re, CancellationToken cancellationToken)
    {
        //var credentials = Credentials.FromApiKeyAndSecret("8d80d3f8", "JKqqb4LNXMVYgre0");
        ////var credentials = Credentials.FromAppIdAndPrivateKeyPath("5b94a771-3056-4621-9e70-03fd636746c0", "C:\\Users\\Bhavesh Sonara\\Downloads\\private (1).key");
        ////var VonageClient = new VonageClient(credentials);

        ////var requestt = new SmsRequest
        ////{
        ////    To = "917990358955",
        ////    From = "Vonage APIs",
        ////    Text = "A text message sent using the Vonage SMS API",
        ////    ClientRef = "abcdefg",
        ////};


        ////var client = new (credentials: new Nexmo.Api.Request.Credentials
        ////{
        ////    ApiKey = "8d80d3f8",
        ////    ApiSecret = "JKqqb4LNXMVYgre0"
        ////});

        ////var client = new NexmoClient(credentials: new Nexmo.Api.Request.Credentials { };

        //var requestt = new WhatsAppTextRequest
        //{
        //    To = "918320015052",
        //    From = "14157386102",
        //    Text = "This is a WhatsApp Message sent from the Messages API"
        //};

        //var client = new VonageClient(credentials);
        //var response = await client.MessagesClient.SendAsync(requestt);


        // Set the API key and API secret
        //string apiKey = "8d80d3f8";
        //string apiSecret = "JKqqb4LNXMVYgre0";
        //var credentials = Credentials.FromAppIdAndPrivateKey("5a2659bc-9058-4d79-8d9f-d18c24d24a31", "-----BEGIN PRIVATE KEY-----MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC5Uq/ig2kx1nq9QUIh+87rtcYeiLc1cuwESa/pR51y/Hjx8ImoH/lpF4vtCXQ6HNZ7P3p3z8aR9qdtr1UUt0NQUPNr8VYkkZFTcjT+gn/HAzAZGUWL0GRmAH2dECZNLdLYoFyP1dqqoRQEZwqAP9x120yhpxjlW66WbVPn2mqWALFGwZIV7PcRuAkI0yOFUmtkOSowKsYbg/Yhx/d4tk7no3sl80fywUHRegidSbt15bRTo6suWw//DaICnK9KKwrnzlnGDkfhwnp9FNerinzKG/Sk//akUfVdbC1ZtiHwpSL5Y3whK4FXQdRN/yeHelfGOmBwxmjHYkKG8nJgq3YzAgMBAAECggEADQSBZVRcwcrykTOxzQhOlyEMGx34XJFHgd+ZMzY8GZUhe8EtMjmYv+iPBCX7W4+mAz9Iv3zBUoR0a1B7FfrviAc/qg/oQKZXAnkNvuj0x8pyCPlM+M65Fq8nI668OgSjgGL3TYU0mHK7ILoKM0sJFeBDKEHADnBbuaZRVAJ0JTglcC8iYNpH9P7JpsovXXZhRSgTKHJKxpdriARntCzqwD2uy1e7YkXWx/lOh61dixI0N6Wxmq6P+IE/jvm7K8CcAKESN/JQkPAmvRGIYpcsFCGAftrDf4pBgujUraBqoICJFpc8z09fplm84zkFBCygAIlTFM8Gf6bACmg02tdbrQKBgQDz01+nQ56l4R0qSsg8DC0jGJdMqW4SYM3qg0BH/VrPbMrJS4T3EEXZHVYjtHJTEH+mqcnQHTj4irIR32wlXGjPeUg5O0GgesyjgmGZpaii1z097DT3y0+ktI5wHh+c4XWDQvxrfVRGafE56Ol2sMXEi65vQlnnPRGcmtkiM9/8fwKBgQDCk4VddkNC6RrZF/kLeO/3qLtYT8wfFzUNLgjpFtX7l6DVKgHWGQz+HCdQYGFyb+xHj8Zg1+a/VkKzsAFmuobFYvP3DmX4N5GQu1hvF4HjG4QsgvK8p7Jff46Lvgsemkj6GWz3Fan3Sv70X7Ibp4pUYW12G/u2TeGO\r\nh5H/9IF8TQKBgCAGg2nU1+Gxz8LeT66TatpRQI4xMuP3ExTaaHcMMAFOqNQHt088M/Bwif/mk52VbS7W3ksXi4QZs4nlbq75lEqsA3CA9/28I2TWmzszJxM8ci7P96UUb0GS9aKEUmKoumajRcRYkdkt0SFqFkAgt9k4/5BdhrF4s/d930c+yQzBAoGAbGV+oKino/j0ygh+55NCLDhF1lbuIkvtdCA3OpNMNlMseFRknX9rK41HoZFv+C699Mf1/jGJDQLjOfNNdItpVUFhJDtTN0rNv1F/XLQ92eWUZq+0xsCtsGIBaXw/+ZZ+HUZY2/WnsGqJSl6dfKaDe6qjLm88dT1B8QM5pNvz3ukCgYEA1bCBvRshFykaaMSaBQDc6rKugeMChad3KrJFKo+1xBlU4xQdfhJMzYOIAEHEzOoZtExHHDPpCdF5/Pd99nfQHaCvn9bjCmHjZMKzlaqwlE7ZbBvU4tZuXhONTR7vuapCHb30et2PBC50nnGCt77dSbIdgCJV1Hdj3nRe1T4ENo8=-----END PRIVATE KEY-----");

        //var request = new WhatsAppTextRequest
        //{
        //    To = "917016768020",
        //    From = "306980829333",
        //    Text = "Hey"
        //};
        //var client = new VonageClient(credentials);

        //var response = await client.MessagesClient.SendAsync(request);


        // send template message

        bool hasBodyParameters = false;

        var CustomerGuestJournies = await _db.GuestJourneyMessagesTemplates.Where(x => x.Id == 1054).FirstOrDefaultAsync(cancellationToken);
        var templeteMessage = CustomerGuestJournies.TempletMessage;
        var buttons = CustomerGuestJournies.Buttons;
        var buttonObject = JsonConvert.DeserializeObject<List<button>>(buttons);

        var ButtonParameters = new List<string>();
        if (buttonObject.Count > 0)
        {
            foreach (var item in buttonObject)
            {
                if(item.Type == "URL")
                {
                    ButtonParameters.Add("https://hospitio.appdemoserver.com/dashboard/onboarding");
                }
                else if(item.Type == "PHONE_NUMBER")
                {
                    ButtonParameters.Add("919664639770");
                }
            }
        }
       

        var bodyplaceholderMatches = System.Text.RegularExpressions.Regex.Matches(templeteMessage, @"\{([^}]+)\}").ToList();

        // Check if there are placeholders in the template content
        hasBodyParameters = (bodyplaceholderMatches.Count > 0) ? true : false;
        int templateMessagePlaceholders = bodyplaceholderMatches.Count;

        var BodyParameters = new List<string>();
        string hotelname = "taj";
        string bookingDays = "2";
        string checkin = "23 sep";
        string checkout = "28 sep";

        string[] placeholderArr = { "hotel_name", "hotel_address", "hotel_phonenumber", "guest_name", "guest_reservationnumber", "checkin_date", "checkout_date", "guest_url" };

        if (bodyplaceholderMatches.Count > 0)
        {
            string[] temparr = new string[bodyplaceholderMatches.Count];
            int count = 0;
            foreach (var item in bodyplaceholderMatches)
            {
                if (item.ToString() == "{hotel_name}")
                {
                    temparr[count] = hotelname;
                }
                else if (item.ToString() == "{hotel_address}")
                {
                    temparr[count] = checkin;
                }
                else if (item.ToString() == "{hotel_phonenumber}")
                {
                    temparr[count] = bookingDays;
                }
                else if (item.ToString() == "{guest_name}")
                {
                    temparr[count] = checkout;
                }
                else if(item.ToString() == "{guest_reservationnumber}")
                {
                    temparr[count] = checkout;
                }
                else if (item.ToString() == "{checkin_date}")
                {
                    temparr[count] = checkout;
                }
                else if (item.ToString() == "{checkout_date}")
                {
                    temparr[count] = checkout;
                }
                else if (item.ToString() == "{guest_url}")
                {
                    temparr[count] = checkout;
                }
                switch (item.ToString())
                {
                    case "{hotel_name}":
                        temparr[count] = hotelname;
                        break;
                    case "{hotel_address}":
                        temparr[count] = hotelname;
                        break;
                    case "{hotel_phonenumber}":
                        temparr[count] = hotelname;
                        break;
                    case "{guest_name}":
                        temparr[count] = hotelname;
                        break;
                    case "{guest_reservationnumber}":
                        temparr[count] = hotelname;
                        break;
                    case "{checkin_date}":
                        temparr[count] = hotelname;
                        break;
                    case "{checkout_date}":
                        temparr[count] = hotelname;
                        break;
                    case "{guest_url}":
                        temparr[count] = hotelname;
                        break;
                }
                count++;
            }
            BodyParameters = temparr.ToList();
        }

        // send template message
        string receiver = "919664639770";
        string sender = "306980829333";
        string templateName = "welcome";

        //var message = _vonageService.SendWhatsappTemplateMessage("5a2659bc-9058-4d79-8d9f-d18c24d24a31", "-----BEGIN PRIVATE KEY-----MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQChLZyclROwPf+satz3AJH2w4dwC5PLsDAvjb2htSJs0QX6cgKUI+A0TGrL1O0x4CjePGvDrlLnxo7ZVjpXgB7NJ2OrQgfQNVdaiVJQI1slwZnnY4xiVC0mQMl958a4FilFaLcrmGkl6VPbzc06+cWGL8ZwnvBSjlYX/U+XUP7ttT7+3sUeDoimPgkGgJzmMDCfoNxXi3DnsrZm6m8oUtUOR06lVx5A7ewoojPJm62KSVtqpnL3jPWxXblR/ErXldhdWLrGI7ADceo0LFR8KagM+6hXxqfBj4DPB3WP3o9qwXaRPZh8GVnKlDxPA9roDDoqZZ0R7O8P+/VbirS7UQGRAgMBAAECggEAMhWnXezhQlnxshU+9q5BrUmTM5kVYy0rvAsyiyZrPR8y2WFGNdx0FixM32waDO6YJH7oCdWIw6cqypSF6pzQdXWw/g21udhpfaPAZVCnSTNA7Os9O2zm3sUxF6PHV3rjdkMU8EIbIoG/4kSwaowk+g6sfmCVU0IRtMCtU9sCbMDwHAJNjnuZ8rn9AopFGt1quckiSavtTBVYCN4eQ8oCXvN7+7ilxGbHhVifTSeXjoC4nxZr9r+TwVgQOqQOl3B6VqK5OVV5p8+EMXmfQeAlwPh3JtVdAX/Nl14B92yx5NHomTnKNSQ2T1wRm7csvxZtuUmNkIJHf4Dh1GC7W//K1QKBgQDVpSVE6skivTrOMoqw88YpYZK3FIPTHR5qzYKRPsTmQj4uSmflwjoKs6hC7Bu0L6QIMHeQkH4YeziZp1VymAMIMJGbhFjRoaIqheoFmmpsqx1JhuIA5pk/y2L/6KzIWbBFqLGqVXxmul+TmEWUNIBDlh6b16ID9wGEj9BkyENVPwKBgQDBIa4ESDlps9CPopRw9GGWBD6xNm5rRR8y5TeOcTZ28xTrHb7Xye5FpJeHukdIyVOKEAVWg9NJJ/e5XvK0jd/y+AfmbLZUFua3FSBe9A6oV7iupeUxv9y1mrIU96AUm698uEu6El6BdKpB3S/VQtYurDiJtK6Nf+02UG2sOI3lLwKBgQCM/3TdSuZ7ms9Yjlqh9gBuBwtA8LUfezQ74G2vVfG01Tscadav98M+lNsTb6fI/zgOf44pRnMxzQDJx3nJKzG1EfjG3k2P7FCOJ9sO354lIbkucWpulcHGLICly/VcNHT1RCQc+lYjphS13+TrrsqH0GdbCrDOVRIXXqJ2IQTvGQKBgQCFVflMH4jzvx8Ya0hMi4vsBFY8BrZI/NnDS5kFkIfnq38fq9OcK1+DWVT8cdDRIZ25TcJBrpVqhlty8Whi2yhoGHFr1lYyy/TRJZbJt3l/I8DvYr1PkYSRJJIaA7PTRoDrfFlbx17TxXXeLxTdCV3RrzkBaWqxakadHv34zrq4JQKBgHZwaeizEbChYwFDpEef7RJwwh/0Db6HLPeqeiBNqSNCmiBBrlPClCoa3CxlgGRs+J3PJD9NG5ACf6k2I0L9qev/QeVqF2vhpMLEQ4upvIsWUoYfMqO13TYs5Swp14ZSD/exE4bMOjearCERIzEES2JqP0u0Rrs1C1cQO2alOCff-----END PRIVATE KEY-----", "", receiver, sender, templateName, BodyParameters, ButtonParameters);


        // Set the recipient phone number
        //string toNumber = "917016768020"; // Replace with the recipient's phone number

        //// Set the message details
        //string fromNumber = "306980829333";
        //string messageText = "Hey Bhavesh";
        //string channel = "whatsapp";

        //// Create the JSON payload
        //string payload = $"{{\"from\": \"{fromNumber}\", \"to\": \"{toNumber}\", \"message_type\": \"text\", \"text\": \"{messageText}\", \"channel\": \"{channel}\"}}";

        //// Set the API endpoint URL
        //string url = "https://messages-sandbox.nexmo.com/v1/messages";

        //// Create the HTTP client and request message
        //HttpClient client = new HttpClient();
        //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

        //// Set the request headers
        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{apiKey}:{apiSecret}")));

        ////request.Headers.Add("Content-Type", "application/json");
        //request.Headers.Add("Accept", "application/json");
        //// Set the request content
        //request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

        //// Send the HTTP request and get the response
        //HttpResponseMessage response = await client.SendAsync(request);

        return _response.Success(new VonageOut("successful."));
    }
}
public class button
{
    public string? Type { get; set; }
    public string? Text { get; set;}
    public string? Value { get; set;}
}