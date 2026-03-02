using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerInfoByWidgetChatId;

public class GetCustomerInfoByWidgetChatIdOut : BaseResponseOut
{
    public GetCustomerInfoByWidgetChatIdOut(string message, GetCustomerInfoByWidgetChatIdResponseOut getCustomerInfoByWidgetChatIdOut) : base(message)
    {
        getCustomerInfoByWidgetChatIdResponseOut = getCustomerInfoByWidgetChatIdOut;
    }
    public GetCustomerInfoByWidgetChatIdResponseOut? getCustomerInfoByWidgetChatIdResponseOut { get; set; }
}

public class GetCustomerInfoByWidgetChatIdResponseOut
{
    public string? Logo { get;set; }
    public string? Color { get;set;}
    public int? CustomerUserId { get;set; }
    public string? ChatWidgetPortal { get;set; }
    public string? Cname { get;set; }
}          
