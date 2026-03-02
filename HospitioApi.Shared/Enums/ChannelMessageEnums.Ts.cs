namespace HospitioApi.Shared.Enums
{
    public enum MessageSenderEnum
    {
        Hospitio = 1,
        Customer = 2,
        Guest = 3,
    }

    public enum MsgReqTypeEnum
    {
        message = 1,
        request = 2,
        journeyMessage = 3,
        alertMessage = 4,
        digitalAssitant = 5,
        welcomeMessage = 6
    }

    public enum ChatUserTypeEnum
    {
        HospitioUser = 1,
        CustomerUser = 2,
        CustomerGuest = 3,
        AnonymousUser = 4,
        ChatWidgetUser = 5,
    }

    public enum RequestTypeEnum
    {
        Common = 1,
        EnhanceYourStay = 2
    }
}
