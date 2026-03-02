using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using HospitioApi.Core.HandleVonage.Commands.InboundWebhook;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.BackGroundServiceData;
using System.Globalization;
using System.Text;

namespace HospitioApi.Core.RabbitMQ;

public class RabbitMQClient : IRabbitMQClient
{
    private readonly ILogger<RabbitMQClient> _logger;
    private static ConnectionFactory _factory;
    private static IConnection _connection;
    private static IModel _model;
    private readonly IBackGroundServiceData _backGroundServiceData;
    private readonly RabbitMQSettingsOptions _rabbitMQ;
    public RabbitMQClient(ILogger<RabbitMQClient> logger, IBackGroundServiceData backGroundServiceData, IOptions<RabbitMQSettingsOptions> rabbiMQ)
    {
        _rabbitMQ = rabbiMQ.Value;
        _logger = logger;
        _backGroundServiceData = backGroundServiceData;
        CreateConnection();
    }
    public void CreateConnection()
    {
        _factory = new ConnectionFactory
        {
            HostName = _rabbitMQ.HostName,
            Port = _rabbitMQ.Port,
            UserName = _rabbitMQ.UserName,
            Password = _rabbitMQ.Password
        };

        _connection = _factory.CreateConnection();
        _model = _connection.CreateModel();
        _model.ExchangeDeclare(exchange: _rabbitMQ.Exchange, type: ExchangeType.Direct);

        _model.QueueDeclare(_rabbitMQ.CustomerQueue, true, false, false, null);
        _model.QueueDeclare(_rabbitMQ.GuestMessageQueue, true, false, false, null);
        _model.QueueDeclare(_rabbitMQ.WPMessageQueue, true, false, false, null);
        //_model.QueueDeclare(_rabbitMQ.AllQueueName, true, false, false, null);

        _model.QueueBind(_rabbitMQ.CustomerQueue, _rabbitMQ.Exchange, "hospitio.customer");
        _model.QueueBind(_rabbitMQ.GuestMessageQueue, _rabbitMQ.Exchange,
            "hospitio.guestmessage");
        _model.QueueBind(_rabbitMQ.WPMessageQueue, _rabbitMQ.Exchange,
            "hospitio.receivewpmessage");
    }
    public async Task SendCustomer(CustomerAction customerAction)
    {
        var json = JsonConvert.SerializeObject(customerAction);
        var message = Encoding.ASCII.GetBytes(json);
        await Send(message, "hospitio.customer");
    }

    public async Task SendGuestMessage(CustomerGuestJorneyDetails guestMessage)
    {
        var json = JsonConvert.SerializeObject(guestMessage);
        var message = Encoding.ASCII.GetBytes(json);
        await Send(message, "hospitio.guestmessage");
    }
    public async Task ReceiveWPMessage(GetInboundWebhookIn getInboundWebhookIn)
    {
        var json = JsonConvert.SerializeObject(getInboundWebhookIn);
        var message = Encoding.ASCII.GetBytes(json);
        await Send(message, "hospitio.receivewpmessage");
    }
    public async Task Send(byte[] message, string routingKey)
    {
        _model.BasicPublish(_rabbitMQ.Exchange, routingKey, null, message);
    }

    public async Task GuestMessage(int CustomerId, int CustomerUserId)
    {
        try
        {
            #region For Refernce
            //GuestMessage guestMessage1 = new GuestMessage();
            CustomerGuestJorneyDetails customerGuestJorneyDetails1 = new CustomerGuestJorneyDetails();
            string dateString = "2024-01-10 08:18:25.837";
            DateTime parsedDateTime = DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            #endregion

            var guestMessages = await _backGroundServiceData.GetGuestMessageByCustomerId(CustomerId, DateTime.UtcNow, CancellationToken.None);
                foreach (var item in guestMessages)
                {
                    customerGuestJorneyDetails1.ActionName = "GUEST_MESSAGE";
                    customerGuestJorneyDetails1.Id = item.Id;
                    customerGuestJorneyDetails1.VonageTemplateId = item.VonageTemplateId;
                    customerGuestJorneyDetails1.VonageTemplateStatus = item.VonageTemplateStatus;
                    customerGuestJorneyDetails1.CustomerId = item.CustomerId;
                    customerGuestJorneyDetails1.GuestId = item.GuestId;
                    customerGuestJorneyDetails1.ReservationNumber = item.ReservationNumber;
                    customerGuestJorneyDetails1.Phone = item.Phone;
                    customerGuestJorneyDetails1.Email = item.Email;
                    customerGuestJorneyDetails1.CheckoutDate = item.CheckoutDate;
                    customerGuestJorneyDetails1.CheckinDate = item.CheckinDate;
                    customerGuestJorneyDetails1.APPId = item.APPId;
                    customerGuestJorneyDetails1.BussinessName = item.BussinessName;
                    customerGuestJorneyDetails1.TemplateName = item.TemplateName;
                    customerGuestJorneyDetails1.Buttons = item.Buttons;
                    customerGuestJorneyDetails1.CustomerUserId = item.CustomerUserId;
                    customerGuestJorneyDetails1.CustomerWhatsAppNumber = item.CustomerWhatsAppNumber;
                    customerGuestJorneyDetails1.PrivateKey = item.PrivateKey;
                    customerGuestJorneyDetails1.TempletMessage = item.TempletMessage;
                    customerGuestJorneyDetails1.IsActive = item.IsActive;
                    customerGuestJorneyDetails1.BussinessAddress = item.BussinessAddress;
                    customerGuestJorneyDetails1.BussinessPhoneNumber = item.BussinessPhoneNumber;
                    customerGuestJorneyDetails1.BookingDays = item.BookingDays;
                    customerGuestJorneyDetails1.GuestName = item.GuestName;
                    customerGuestJorneyDetails1.GuestURL = item.GuestURL;
                    customerGuestJorneyDetails1.EligibleForWhatsappCommunication = item.EligibleForWhatsappCommunication;
                    customerGuestJorneyDetails1.EligibleForSMSCommunication = item.EligibleForSMSCommunication;
                    customerGuestJorneyDetails1.EligibleForEmailCommunication = item.EligibleForEmailCommunication;
                    customerGuestJorneyDetails1.EligibleForWebChatCommunication = item.EligibleForWebChatCommunication;
                    customerGuestJorneyDetails1.CustomerLogoURL = item.CustomerLogoURL;
                    customerGuestJorneyDetails1.CustomerColour = item.CustomerColour;
                    customerGuestJorneyDetails1.SMSTwoWayCommunication = item.SMSTwoWayCommunication;

                    await SendGuestMessage(customerGuestJorneyDetails1);
                }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Get Data.");
        }
    }
}

